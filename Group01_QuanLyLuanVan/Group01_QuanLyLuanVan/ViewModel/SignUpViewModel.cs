using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group01_QuanLyLuanVan.View;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    { 
        private string _linkaddimage;
        public string linkaddimage { get => _linkaddimage; set { _linkaddimage = value; OnPropertyChanged(); } }

        public ICommand Register { get; set; }
        public ICommand AddImage { get; set; }

        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }
        public ICommand PasswordChangedCommand { get; set; }



        public SignUpViewModel()
        {
            linkaddimage = Const._localLink + "/Resource/Image/addava.png";
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            AddImage = new RelayCommand<ImageBrush>((p) => true, (p) => _AddImage(p));
            Register = new RelayCommand<SignUpView>((p) => true, (p) => _Register(p));
        }

        void _AddImage(ImageBrush img)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.png)|*.jpg; *.png";
            if (open.ShowDialog() == true)
            {
                if (open.FileName != "")
                    linkaddimage = open.FileName;
            };
            Uri fileUri = new Uri(linkaddimage);
            img.ImageSource = new BitmapImage(fileUri);
        }

        void _Register(SignUpView parameter)
        {
            if (parameter.username.Text == "" || password == "" || parameter.sinhVienId.Text == "" || parameter.tenKhoa.Text == "" || parameter.ngaySinh.SelectedDate == null || parameter.hoTen.Text == "" || parameter.gioiTinh.Text == "" || parameter.sdt.Text == "" || parameter.mail.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DataProvider.Ins.DB.TaiKhoans.Any(x => x.username == parameter.username.Text))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateTime.Now.Year - parameter.ngaySinh.SelectedDate.Value.Year < 18)
            {
                MessageBox.Show("Số tuổi phải lớn hơn 18 !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match1 = @"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            Regex reg1 = new Regex(match1);
            if (!reg1.IsMatch(parameter.sdt.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DataProvider.Ins.DB.TaiKhoans.Any(x => x.mail == parameter.mail.Text))
            {
                MessageBox.Show("Email này đã được sử dụng!", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string match = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";
            Regex reg = new Regex(match);
            if (!reg.IsMatch(parameter.mail.Text))
            {
                MessageBox.Show("Email không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TaiKhoan tk = new TaiKhoan();
            tk.username = parameter.username.Text;
            tk.password = password;
            tk.mail = parameter.mail.Text;
            tk.quyen = 2;
            tk.trangThai = 0;
            Random rand = new Random();
            tk.code = rand.Next(100000, 999999).ToString();
            SinhVien sv = new SinhVien();
            sv.id = int.Parse(parameter.sinhVienId.Text);
            sv.khoaId = "K01";
            sv.hoTen = parameter.hoTen.Text;
            sv.SDT = parameter.sdt.Text;
            sv.gioiTinh = parameter.gioiTinh.Text;
            sv.ngaySinh = DateTime.Parse(parameter.ngaySinh.Text.ToString());
            sv.username = parameter.username.Text;
            sv.email = parameter.mail.Text;


            if (linkaddimage == "/Resource/Image/addava.png")
                tk.avatar = "/Resource/Image/addava.png";
            else
                tk.avatar = "/Resource/Ava/" + tk.username + ((linkaddimage.Contains(".jpg")) ? ".jpg" : ".png").ToString();
            Const.taiKhoan = tk;
            SendCode(tk.mail, tk.code);

            DataProvider.Ins.DB.TaiKhoans.Add(tk);
            DataProvider.Ins.DB.SaveChanges();

            DataProvider.Ins.DB.SinhViens.Add(sv);
            DataProvider.Ins.DB.SaveChanges();
            try
            {
               
            File.Copy(linkaddimage, Const._localLink + @"/Resource/Ava/" + tk.username + ((linkaddimage.Contains(".jpg")) ? ".jpg" : ".png").ToString(), true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            Window oldWindow = App.Current.MainWindow;
            oldWindow.Hide();
            VerifyCodeView verifyCodeView = new VerifyCodeView();
            oldWindow.Close();
            App.Current.MainWindow = verifyCodeView;
            verifyCodeView.ShowDialog();
        }
        void SendCode(string mail, string code)
        {
            string nd = "Vui lòng nhập code: " + code + " để đăng ký tài khoản. Trân trọng !";
            MailMessage message = new MailMessage("21110587@student.hcmute.edu.vn", mail, "Xác nhận đăng ký", nd);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("21110587@student.hcmute.edu.vn", "ngahungA@963");
            smtpClient.Send(message);
        }
    }
}
