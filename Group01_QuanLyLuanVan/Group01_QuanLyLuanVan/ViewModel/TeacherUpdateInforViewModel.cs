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
using Group01_QuanLyLuanVan.DAO;
using System.IO;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherUpdateInforViewModel : BaseViewModel
    {
        KhoaDAO khoaDAO = new KhoaDAO();
        GiangVienDAO gvDAO = new GiangVienDAO();
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();

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
            if (Const.taiKhoan.Avatar == "/Resource/Image/addava.png")
                Ava = Const._localLink + "/Resource/Image/addava.png";
            else
                Ava = Const._localLink + Const.taiKhoan.Avatar;
            GiangVien gv = Const.giangVien;
            GiangVienId = gv.GiangVienId;
            NgaySinh = gv.NgaySinh.ToString();
            DiaChi = gv.DiaChi;
            GioiTinh = (gv.GioiTinh == "Nam") ? 0 : 1;
            SDT = gv.SDT;
            HoTen = gv.HoTen;
            Mail = gv.Email;
            Khoa khoa = khoaDAO.FindByKhoaId(gv.KhoaId);
            if (khoa.TenKhoa == "Công nghệ thông tin")
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
            GiangVien gv = new GiangVien();
            gv.GiangVienId = GiangVienId;
            gv.KhoaId = TenKhoa.ToString();
            if (TenKhoa == 0)
                gv.KhoaId = "K01";
            else gv.KhoaId = "K02";
            gv.HoTen = HoTen;
            gv.GioiTinh = (GioiTinh == 0) ? "Nam" : "Nữ";
            gv.NgaySinh = DateTime.Parse(NgaySinh);
            gv.SDT = SDT;
            gv.Email = Mail;
            gv.DiaChi = DiaChi;
            gvDAO.UpdateGiangVien(gv);
            Const.giangVien = gv;
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

            // Cập nhật tài khoản với Avatar mới
            tkDAO.UpdateTaiKhoan(Const.taiKhoan.Mail, Const.taiKhoan.Avatar, Const.taiKhoan.Username);
            Window oldWindow = App.Current.MainWindow;
            TeacherMainView teacherMainView = new TeacherMainView();

            App.Current.MainWindow = teacherMainView;
            teacherMainView.Show();
            Const.taiKhoan = tkDAO.FindOneByUsername(Const.taiKhoan.Username);
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
