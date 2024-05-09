using Group01_QuanLyLuanVan.Chat.Net;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class LoginViewModel : BaseViewModel
    { 
        private string username;
        public string Username { get => username; set { username = value; OnPropertyChanged(); } }

        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged(); } }

        public static Frame MainFrame { get; set; }
        public ICommand LoginCM { get; set; }
        public ICommand LoadLoginPageCM { get; set; }
        public ICommand ForgetPasswordCM { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        public LoginViewModel()
        {
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });

            LoadLoginPageCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                MainFrame = p;
                p.Content = new LoginPageView();
            });

            LoginCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                try
                {
                    TaiKhoan tk = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(x => x.username == Username && x.password == Password);

                    Const.taiKhoan = tk;

                    if (tk != null)
                    {
                        if (tk.trangThai == 0)
                        {
                            MessageBox.Show("Tài khoản chưa được kích hoạt!", "Thông báo", MessageBoxButton.OK);
                        }
                        else
                        {
                            if (tk.quyen == 0)
                            {
                                Window oldWindow = App.Current.MainWindow;
                                ADMainView adMainView = new ADMainView();
                                App.Current.MainWindow = adMainView;
                                oldWindow.Close();
                                adMainView.Show();
                            }
                            else if (tk.quyen == 1)
                            {

                                GiangVien gv = DataProvider.Ins.DB.GiangViens.FirstOrDefault(x => x.TaiKhoan.username == Const.taiKhoan.username);

                                Const.giangVien = gv;
                                Const._server = new Server();
                                Const._server.ConnectToServer(Const.giangVien.username);
                                Window oldWindow = App.Current.MainWindow;
                                TeacherMainView teacherMainView = new TeacherMainView();
                                App.Current.MainWindow = teacherMainView;
                                oldWindow.Close();
                                teacherMainView.Show();
                            }
                            else
                            {

                                SinhVien sv = DataProvider.Ins.DB.SinhViens.FirstOrDefault(x => x.TaiKhoan.username == Const.taiKhoan.username);

                                Const.sinhVien = sv;
                                Const._server = new Server();
                                Const._server.ConnectToServer(Const.sinhVien.username);
                                Window oldWindow = App.Current.MainWindow;
                                StudentMainView studentMainView = new StudentMainView();
                                App.Current.MainWindow = studentMainView;
                                oldWindow.Close();
                                studentMainView.Show();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButton.OK);
                    }
                }
                catch
                {
                    MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK);
                }
            })
            {

            };
            ForgetPasswordCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new ForgetPasswordView();
            });

        }
    }
}
