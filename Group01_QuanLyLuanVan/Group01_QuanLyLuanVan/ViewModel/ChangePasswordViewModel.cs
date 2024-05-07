using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        TaiKhoanDAO tkDAO= new TaiKhoanDAO();
        private string _OldPass;
        public string OldPass { get => _OldPass; set { _OldPass = value; OnPropertyChanged(); } }
        private string _NewPass;
        public string NewPass { get => _NewPass; set { _NewPass = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand CancelCM { get; set; }
        public ICommand OldPassChangedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand NewPassChangedCommand { get; set; }
        public ICommand SaveCM { get; set; }

        public ChangePasswordViewModel()
        {
            CancelCM = new RelayCommand<ChangePasswordView>((p) => true, (p) => _CancelCM());
            SaveCM = new RelayCommand<ChangePasswordView>((p) => true, (p) => _SaveNewPass(p));
            OldPassChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { OldPass = p.Password; });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            NewPassChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { NewPass = p.Password; });
            
        }

        void _CancelCM()
        {
            TeacherUpdateInforViewModel teacherUpdateInforViewModel = new TeacherUpdateInforViewModel();
            TeacherMainViewModel.MainFrame.Content = teacherUpdateInforViewModel;
        }

        void _SaveNewPass(ChangePasswordView p)
        {
            TaiKhoan tk = Const.taiKhoan;
            try
            {
                if (Password == "" || OldPass == "" || NewPass == "")
                {
                    MessageBox.Show("Vui lòng nhập thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if ( tk.Password != OldPass)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (Password == OldPass)
                {
                    MessageBox.Show("Mật khẩu mới không được giống mật khẩu cũ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (Password != NewPass)
                {
                    MessageBox.Show("Mật khẩu nhập lại không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    tkDAO.ChangePassword(tk.Username, NewPass);
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
