using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.IO;
using System.Data.Entity;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherUpdateInforViewModel : BaseViewModel
    {

        private string _Ava;
        public string Ava { get => _Ava; set { _Ava = value; OnPropertyChanged(); } }
        private string giangVienId;
        public string GiangVienId { get => giangVienId; set { giangVienId = value; OnPropertyChanged(); } }
        private string hoTen;
        public string HoTen { get => hoTen; set { hoTen = value; OnPropertyChanged(); } }
        private string diaChi;
        public string DiaChi { get => diaChi; set { diaChi = value; OnPropertyChanged(); } }
        private string mail;
        public string Mail { get => mail; set { mail = value; OnPropertyChanged(); } }

        private int tenKhoa;
        public int TenKhoa { get => tenKhoa; set { tenKhoa = value; OnPropertyChanged(); } }

        private int gioiTinh;
        public int GioiTinh { get => gioiTinh; set { gioiTinh = value; OnPropertyChanged(); } }
        private string sdt;
        public string SDT { get => sdt; set { sdt = value; OnPropertyChanged(); } }
        private string ngaySinh;
        public string NgaySinh { get => ngaySinh; set { ngaySinh = value; OnPropertyChanged(); } }

        public ICommand Loadwd { get; set; }
        public ICommand UpdateInfo { get; set; }
        public ICommand AddImage { get; set; }
        public ICommand ChangePass { get; set; }

        public TeacherUpdateInforViewModel()
        {
            Loadwd = new RelayCommand<TeacherUpdateInforView>((p) => true, (p) => _Loadwd(p));
            AddImage = new RelayCommand<ImageBrush>((p) => true, (p) => _AddImage(p));
            UpdateInfo = new RelayCommand<TeacherUpdateInforView>((p) => true, (p) => _UdpateInfo(p));
            ChangePass = new RelayCommand<TeacherUpdateInforView>((p) => true, (p) => _ChangePass());
        }
        void _AddImage(ImageBrush img)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.png)|*.jpg; *.png";

            if (open.ShowDialog() == true)
            {
                if (open.FileName != "")
                    Ava = open.FileName;
            };
            Uri fileUri = new Uri(Ava);
            img.ImageSource = new BitmapImage(fileUri);
        }

        void _Loadwd(TeacherUpdateInforView p)
        {
            if (Const.taiKhoan.avatar == "/Resource/Image/addava.png")
                Ava = Const._localLink + "/Resource/Image/addava.png";
            else
                Ava = Const._localLink + Const.taiKhoan.avatar;
            GiangVien gv = Const.giangVien;
            GiangVienId = gv.giangVienId;
            NgaySinh = gv.ngaySinh.ToString();
            DiaChi = gv.diaChi;
            GioiTinh = (gv.gioiTinh == "Nam") ? 0 : 1;
            SDT = gv.SDT;
            HoTen = gv.hoTen;
            Mail = gv.email;
            Khoa khoa = DataProvider.Ins.DB.Khoas.FirstOrDefault(x => x.khoaId == gv.khoaId);
            if (khoa.tenKhoa == "Công nghệ thông tin")
                TenKhoa = 0;
            else TenKhoa = 1;
        }

        void _UdpateInfo(TeacherUpdateInforView p)
        {
            if (GiangVienId == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (HoTen == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match1 = @"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            Regex reg1 = new Regex(match1);
            if (!reg1.IsMatch(SDT))
            {
                MessageBox.Show("Số điện thoại không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";
            Regex reg = new Regex(match);
            if (!reg.IsMatch(Mail))
            {
                MessageBox.Show("Email không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GiangVien gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.giangVienId == GiangVienId);
            if (gv != null)
            {
                gv.giangVienId = GiangVienId;
                gv.khoaId = TenKhoa.ToString();
                if (TenKhoa == 0)
                    gv.khoaId = "K01";
                else gv.khoaId = "K02";
                gv.hoTen = HoTen;
                gv.gioiTinh = (GioiTinh == 0) ? "Nam" : "Nữ";
                gv.ngaySinh = DateTime.Parse(NgaySinh);
                gv.SDT = SDT;
                gv.email = Mail;
                gv.diaChi = DiaChi;
                try
                {
                    DataProvider.Ins.DB.GiangViens.Attach(gv);
                    DataProvider.Ins.DB.SaveChanges();
                }

                catch (Exception ec)
                {
                    Console.WriteLine(ec.Message);
                }
                DataProvider.Ins.DB.SaveChanges();
            }
            Const.giangVien = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.username == Const.taiKhoan.username);

            Const.taiKhoan.mail = Mail;
            string avatarFileName = Const.taiKhoan.username + ((Ava.Contains(".jpg")) ? ".jpg" : ".png").ToString();
            string avatarPath = Const._localLink + @"/Resource/Ava/";

            if (File.Exists(avatarPath + avatarFileName))
            {
                string newAvatarFileName = GetUniqueFileName(avatarFileName);
                File.Copy(Ava, avatarPath + newAvatarFileName, true);
                Const.taiKhoan.avatar = "/Resource/Ava/" + newAvatarFileName;
            }
            else
            {
                File.Copy(Ava, avatarPath + avatarFileName, true);
                Const.taiKhoan.avatar = "/Resource/Ava/" + avatarFileName;
            }

            TaiKhoan tk = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(x => x.username == Const.taiKhoan.username);
            if (tk != null)
            {
                tk.mail = Const.taiKhoan.mail;
                tk.avatar = Const.taiKhoan.avatar;
                DataProvider.Ins.DB.SaveChanges();
            }
            Const.taiKhoan = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(x => x.username == Const.taiKhoan.username);
            Const.giangVien = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.username == Const.taiKhoan.username);

            Window oldWindow = App.Current.MainWindow;
            TeacherMainView teacherMainView = new TeacherMainView();
            App.Current.MainWindow = teacherMainView;
            teacherMainView.Show();
            oldWindow.Close();
        }

        FrameworkElement GetParentWindow(FrameworkElement p)
            {
                FrameworkElement parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }
                return parent;
            }
        void _ChangePass()
        {
            ChangePasswordView changePasswordView = new ChangePasswordView();
            TeacherMainViewModel.MainFrame.Content = changePasswordView;
        }

        private string GetUniqueFileName(string fileName)
        {
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string uniqueName = nameWithoutExtension + "_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
            return uniqueName;
        }
    }
}
