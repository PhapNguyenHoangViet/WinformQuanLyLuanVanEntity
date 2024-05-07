using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Group01_QuanLyLuanVan.View
{
    /// <summary>
    /// Interaction logic for LoginPageView.xaml
    /// </summary>
    public partial class LoginPageView : Page
    {
        public LoginPageView()
        {
            InitializeComponent();
        }

        private void OpenSignUp(object sender, RoutedEventArgs e)
        {
            Window oldWindow = App.Current.MainWindow;
            SignUpView signUpView = new SignUpView();
            App.Current.MainWindow = signUpView;
            signUpView.ShowDialog();
        }
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            // Khi CheckBox được kiểm tra, hiển thị mật khẩu
            Password11.Visibility = Visibility.Visible;
            Password11.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            // Khi CheckBox bị bỏ kiểm tra, ẩn mật khẩu
            PasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Password = Password11.Text;
            Password11.Visibility = Visibility.Collapsed;
        }

        private void Password11_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBox.Password = Password11.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password11.Text = PasswordBox.Password;
        }

    }
}
