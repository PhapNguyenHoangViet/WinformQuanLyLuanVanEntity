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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Data;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentUpdateInforViewModel : BaseViewModel
    {


        private string _Ava;
        public string Ava { get => _Ava; set { _Ava = value; OnPropertyChanged(); } }
        private string sinhVienId;
        public string SinhVienId { get => sinhVienId; set { sinhVienId = value; OnPropertyChanged(); } }
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
        public StudentUpdateInforViewModel()
        {
            Loadwd = new RelayCommand<StudentUpdateInforView>((p) => true, (p) => _Loadwd(p));
            AddImage = new RelayCommand<ImageBrush>((p) => true, (p) => _AddImage(p));
            UpdateInfo = new RelayCommand<StudentUpdateInforView>((p) => true, (p) => _UdpateInfo(p));
            ChangePass = new RelayCommand<StudentUpdateInforView>((p) => true, (p) => _ChangePass());
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
        void _Loadwd(StudentUpdateInforView p)
        {
            if (Const.taiKhoan.avatar == "/Resource/Image/addava.png")
                Ava = Const._localLink + "/Resource/Image/addava.png";
            else
                Ava = Const._localLink + Const.taiKhoan.avatar;
            SinhVien sv = Const.sinhVien;
            SinhVienId = sv.sinhVienId;
            NgaySinh = sv.ngaySinh.ToString();
            DiaChi = sv.diaChi;
            GioiTinh = (sv.gioiTinh == "Nam") ? 0 : 1;
            SDT = sv.SDT;
            HoTen = sv.hoTen;
            Mail = sv.email;
            var khoa = DataProvider.Ins.DB.Khoas.FirstOrDefault(k => k.khoaId == Const.sinhVien.khoaId);
            if (khoa.tenKhoa == "Công nghệ thông tin")
                TenKhoa = 0;
            else TenKhoa = 1;
        }

        void _UdpateInfo(StudentUpdateInforView p)
        {
            if (SinhVienId == "")
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
            SinhVien sv = new SinhVien();

            var sinhVien = DataProvider.Ins.DB.SinhViens.FirstOrDefault(s => s.sinhVienId == sv.sinhVienId);

            if (sinhVien != null)
            {
                sinhVien.sinhVienId = SinhVienId;
                sinhVien.khoaId = TenKhoa.ToString();
                if (TenKhoa == 0)
                    sinhVien.khoaId = "K01";
                else sinhVien.khoaId = "K02";
                sinhVien.hoTen = HoTen;
                sinhVien.gioiTinh = (GioiTinh == 0) ? "Nam" : "Nữ";
                sinhVien.ngaySinh = DateTime.Parse(NgaySinh);
                sinhVien.SDT = SDT;
                sinhVien.email = Mail;
                sinhVien.diaChi = DiaChi;
                DataProvider.Ins.DB.SaveChanges();
            }
            Const.sinhVien = DataProvider.Ins.DB.SinhViens.FirstOrDefault(sv1 => sv1.username == Const.taiKhoan.username);


            Const.taiKhoan.mail = mail;
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

            var taiKhoan = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(tk => tk.username == Const.taiKhoan.username);

            if (taiKhoan != null)
            {
                taiKhoan.mail = Const.taiKhoan.mail;
                taiKhoan.avatar = Const.taiKhoan.avatar;
                DataProvider.Ins.DB.SaveChanges();
            }

            Const.taiKhoan = DataProvider.Ins.DB.TaiKhoans
                        .FirstOrDefault(tk => tk.username == Const.taiKhoan.username);


            Window oldWindow = App.Current.MainWindow;
            StudentMainView studentMainView = new StudentMainView();
            App.Current.MainWindow = studentMainView;
            studentMainView.Show();
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
            StudentMainViewModel.MainFrame.Content = changePasswordView;
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
