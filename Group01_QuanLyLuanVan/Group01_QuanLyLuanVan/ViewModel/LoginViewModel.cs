using Group01_QuanLyLuanVan.Chat.Net;
using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        SinhVienDAO svDAO = new SinhVienDAO();
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();
        GiangVienDAO gvDAO = new GiangVienDAO();

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
                    TaiKhoan tk = tkDAO.FindOne(Username, Password);
                    Const.taiKhoan = tk;

                    if (tk != null)
                    {
                        if (tk.TrangThai == 0)
                        {
                            MessageBox.Show("Tài khoản chưa được kích hoạt!", "Thông báo", MessageBoxButton.OK);
                        }
                        else 
                        {
                            if (tk.Quyen == 0)
                            {
                                Window oldWindow = App.Current.MainWindow;
                                ADMainView adMainView = new ADMainView();
                                App.Current.MainWindow = adMainView;
                                oldWindow.Close();
                                adMainView.Show();
                            }
                            else if (tk.Quyen == 1)
                            {
                                
                                GiangVien gv = gvDAO.FindOneByUsername(Const.taiKhoan.Username);
                                Const.giangVien = gv;
                                Const._server = new Server();
                                Const._server.ConnectToServer(Const.giangVien.Username);
                                Window oldWindow = App.Current.MainWindow;
                                TeacherMainView teacherMainView = new TeacherMainView();   
                                App.Current.MainWindow = teacherMainView;
                                oldWindow.Close();
                                teacherMainView.Show();
                            }
                            else
                            {

                                SinhVien sv = svDAO.FindOneByUsername(Username);
                                Const.sinhVien = sv;
                                Const._server = new Server();
                                Const._server.ConnectToServer(Const.sinhVien.Username);
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
            });

            ForgetPasswordCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainFrame.Content = new ForgetPasswordView();
            });

        }
    }
}
