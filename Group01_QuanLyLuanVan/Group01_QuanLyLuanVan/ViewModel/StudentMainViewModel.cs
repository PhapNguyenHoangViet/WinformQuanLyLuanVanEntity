using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;


namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentMainViewModel : BaseViewModel
    {
        private int _sum;
        public int Sum
        {
            get { return _sum; }
            set
            {
                if (_sum != value)
                {
                    _sum = value;
                    OnPropertyChanged("Sum");
                }
            }
        }
        private string ava;
        public string Ava { get => ava; set { ava = value; OnPropertyChanged(); } }
        private string tenDangNhap;
        public string TenDangNhap { get => tenDangNhap; set { tenDangNhap = value; OnPropertyChanged(); } }
        public ICommand Loadwd { get; set; }
        public ICommand StudentUpdateInforCM { get; set; }
        private string _selectedOption;
        public string SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }
        public ICommand LoadPageHomeCM { get; set; }
        public static Frame MainFrame { get; set; }
        public ICommand HomeStudentCM { get; set; }
        public ICommand StudentRegisterCM { get; set; }
        public ICommand StudentUpdateTaskCM { get; set; }
        public ICommand StudentUpdateProgressCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public ICommand StudentNotiCM { get; set; }

        public ICommand StudentMailCM { get; set; }
        public ICommand StudentScoreCM { get; set; }
        public ICommand StudentThesisCM { get; set; }


        public ObservableCollection<DeTai> Topics { get; set; }
        public void LoadTenND(StudentMainView p)
        {
            p.TenDangNhap.Text = Const.sinhVien.hoTen;
        }

        public StudentMainViewModel()
        {
            Loadwd = new RelayCommand<StudentMainView>((p) => true, (p) => _Loadwd(p));

            // Khởi tạo command để load giao diện ban đầu
            LoadPageHomeCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                LoadTrangthai();
                MainFrame = p;
                p.Content = new HomeView();
            });
            HomeStudentCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                MainFrame.Content = new HomeView();
            });
            StudentRegisterCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                MainFrame.Content = new StudentListTopicView();

            });

            StudentThesisCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                GiangVien gv = new GiangVien();
                if (Const.sinhVien.nhomId.ToString() != "-1")
                {
                    DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
                    string dtTaiId = dt.deTaiId;
                    string gvId = dt.giangVienId;
                    gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.giangVienId == gvId);
                }
                else
                {
                    MessageBox.Show("Vui lòng đăng kí đề tài.");
                    return;
                }
                MainFrame.Content = new StudentThesisView();
            });
            StudentUpdateTaskCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                GiangVien gv = new GiangVien();
                if (Const.sinhVien.nhomId.ToString() != "-1")
                {
                    DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
                    string dtTaiId = dt.deTaiId;
                    string gvId = dt.giangVienId;
                    gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.giangVienId == gvId);
                }
                else
                {
                    MessageBox.Show("Vui lòng đăng kí đề tài.");
                    return;
                }
                MainFrame.Content = new StudentUpdateTaskView();

            });
            StudentUpdateProgressCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                MainFrame.Content = new StudentUpdateProgressView();

            });

            StudentScoreCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                GiangVien gv = new GiangVien();
                if (Const.sinhVien.nhomId.ToString() != "-1")
                {
                    DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
                    string dtTaiId = dt.deTaiId;
                    string gvId = dt.giangVienId;
                    gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.giangVienId == gvId);
                }
                else
                {
                    MessageBox.Show("Vui lòng đăng kí đề tài.");
                    return;
                }
                MainFrame.Content = new StudentScoreView();

            });

            StudentNotiCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                GiangVien gv = new GiangVien();
                if (Const.sinhVien.nhomId.ToString() != "-1")
                {
                    DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
                    string dtTaiId = dt.deTaiId;
                    string gvId = dt.giangVienId;
                    gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.giangVienId == gvId);
                }
                else
                {
                    MessageBox.Show("Vui lòng đăng kí đề tài.");
                    return;
                }
                MainFrame.Content = new StudentNotiView();
            });
            StudentUpdateInforCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                MainFrame.Content = new StudentUpdateInforView();

            });

            StudentMailCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                LoadTrangthai();
                StudentMailView studentMailView = new StudentMailView();
                GiangVien gv = new GiangVien();
                if (Const.sinhVien.nhomId.ToString() != "-1")
                {
                    DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
                    string dtTaiId = dt.deTaiId;
                    string gvId = dt.giangVienId;
                    gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.giangVienId == gvId);
                }
                else
                {
                    MessageBox.Show("Vui lòng đăng kí đề tài.");
                    return;
                }
                if (gv != null)
                {
                    studentMailView.HoTen.Text = gv.hoTen.ToString();
                    studentMailView.EmailAddress.Text = gv.email.ToString();
                }
                else
                {
                    studentMailView.HoTen.Text = "";
                    studentMailView.EmailAddress.Text = "";
                }
                MainFrame.Content = studentMailView;
            });
            SignoutCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                LoadTrangthai();
                FrameworkElement window = GetParentWindow(p);
                Window oldWindow = App.Current.MainWindow;
                LoginView loginView = new LoginView();
                App.Current.MainWindow = loginView;
                oldWindow.Close();
                loginView.Show();
            });
            FrameworkElement GetParentWindow(FrameworkElement p)
            {
                FrameworkElement parent = p;

                while (parent.Parent != null)
                {
                    parent = parent.Parent as FrameworkElement;
                }
                return parent;
            }
        }
        void _Loadwd(StudentMainView p)
        {
            if (Const.taiKhoan.avatar == "/Resource/Image/addava.png")
                Ava = Const._localLink + "/Resource/Ava/addava.png";
            else
                Ava = Const._localLink + Const.taiKhoan.avatar;
            LoadTenND(p);

            LoadTrangthai();

        }
        void LoadTrangthai()
        {
            DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
            string dtTaiId = dt.deTaiId;
            Sum = DataProvider.Ins.DB.ThongBaos 
                .Where(tb => tb.trangthai == 0 && tb.deTaiId == dtTaiId)
                .Count();
           

        }

    }

}
