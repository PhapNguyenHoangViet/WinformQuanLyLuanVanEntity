using Group01_QuanLyLuanVan.DAO;
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


namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentUpdateInforViewModel : BaseViewModel
    {
        KhoaDAO khoaDAO = new KhoaDAO();
        SinhVienDAO svDAO = new SinhVienDAO();
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();

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
            if (Const.taiKhoan.Avatar == "/Resource/Image/addava.png")
                Ava = Const._localLink + "/Resource/Image/addava.png";
            else
                Ava = Const._localLink + Const.taiKhoan.Avatar;
            SinhVien sv = Const.sinhVien;
            SinhVienId = sv.SinhVienId;
            NgaySinh = sv.NgaySinh.ToString();
            DiaChi = sv.DiaChi;
            GioiTinh = (sv.GioiTinh == "Nam") ? 0 : 1;
            SDT = sv.SDT;
            HoTen = sv.HoTen;
            Mail = sv.Email;
            Khoa khoa = khoaDAO.FindByKhoaId(sv.KhoaId);
            if (khoa.TenKhoa == "Công nghệ thông tin")
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
            sv.SinhVienId = SinhVienId;
            sv.KhoaId = TenKhoa.ToString();
            if (TenKhoa == 0)
                sv.KhoaId = "K01";
            else sv.KhoaId = "K02";
            sv.HoTen = HoTen;
            sv.GioiTinh = (GioiTinh == 0) ? "Nam" : "Nữ";
            sv.NgaySinh = DateTime.Parse(NgaySinh);
            sv.SDT = SDT;
            sv.Email = Mail;
            sv.DiaChi = DiaChi;
            svDAO.UpdateSinhVien(sv);
            Const.sinhVien = svDAO.FindOneByUsername(Const.taiKhoan.Username);
            Const.taiKhoan.Mail = Mail;
            string avatarFileName = Const.taiKhoan.Username + ((Ava.Contains(".jpg")) ? ".jpg" : ".png").ToString();
            string avatarPath = Const._localLink + @"/Resource/Ava/";

            if (File.Exists(avatarPath + avatarFileName))
            {
                string newAvatarFileName = GetUniqueFileName(avatarFileName);
                File.Copy(Ava, avatarPath + newAvatarFileName, true);
                Const.taiKhoan.Avatar = "/Resource/Ava/" + newAvatarFileName;
            }
            else
            {
                // Nếu không trùng tên, sao chép file như bình thường
                File.Copy(Ava, avatarPath + avatarFileName, true);
                Const.taiKhoan.Avatar = "/Resource/Ava/" + avatarFileName;
            }
            tkDAO.UpdateTaiKhoan(Const.taiKhoan.Mail, Const.taiKhoan.Avatar, Const.taiKhoan.Username);

            Const.taiKhoan = tkDAO.FindOneByUsername(Const.taiKhoan.Username);

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
