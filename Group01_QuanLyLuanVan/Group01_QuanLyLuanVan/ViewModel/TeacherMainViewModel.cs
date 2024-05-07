using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Group01_QuanLyLuanVan.Properties;
using Group01_QuanLyLuanVan.DAO;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherMainViewModel : BaseViewModel
    {
        private string ava;
        public string Ava { get => ava; set { ava = value; OnPropertyChanged(); } }
        private string tenDangNhap;
        public string TenDangNhap { get => tenDangNhap; set { tenDangNhap = value; OnPropertyChanged(); } }
        public ICommand Loadwd { get; set; }
        public static Frame MainFrame { get; set; }
        public ICommand LoadPageCM { get; set; }
        public ICommand TeacherUpdateInforCM { get; set; }
        public ICommand SignoutCM { get; set; }
        public ICommand HomeCM { get; set; }
        public ICommand TeacherDissertationCM { get; set; }
        public ICommand TeacherTaskCM { get; set; }
        public ICommand TeacherProgressCM { get; set; }
        public ICommand TeacherNotiCM { get; set; }
        public ICommand TeacherMailCM { get; set; }
        
        public ICommand TeacherScoreCM { get; set; }

        public ICommand TeacherStatisticCM { get; set; }


        public void LoadTenND(TeacherMainView p)
        {
            p.TenDangNhap.Text = Const.giangVien.HoTen;
        }

        public TeacherMainViewModel()
        {

            Loadwd = new RelayCommand<TeacherMainView>((p) => true, (p) => _Loadwd(p));

            LoadPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame = p;
                p.Content = new HomeView();
            });

            HomeCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {
                MainFrame.Content = new HomeView();
            });

            TeacherUpdateInforCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherUpdateInforView();
            });

            TeacherDissertationCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherDissertationView();
            });

            TeacherTaskCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherTaskView();
            });

            TeacherScoreCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherScoreView();
            });

            TeacherProgressCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherProgressView();
            });

            TeacherNotiCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherNotiView();
            });

            TeacherStatisticCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new TeacherStatisticView();
            });

            TeacherMailCM = new RelayCommand<Frame>((P) => { return true; }, (P) =>
            {

                TeacherMailView teacherMailView = new TeacherMailView();
                GiangVien gv = new GiangVien();
                
                if (gv != null)
                {
                    teacherMailView.HoTen.Text = "";
                    teacherMailView.EmailAddress.Text = "";
                }
                MainFrame.Content = teacherMailView;

            });
            SignoutCM = new RelayCommand<FrameworkElement>((p) => { return p == null ? false : true; }, (p) =>
            {
                Window oldWindow = App.Current.MainWindow;
                LoginView loginView = new LoginView();
                App.Current.MainWindow = loginView;
                oldWindow.Close();
                loginView.Show();
            });
        }

        void _Loadwd(TeacherMainView p)
        {
            if (Const.taiKhoan.Avatar == "/Resource/Image/addava.png")
                Ava = Const._localLink + "/Resource/Ava/addava.png";
            else
                Ava = Const._localLink + Const.taiKhoan.Avatar;
            LoadTenND(p);
        }
    }
}
