using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class ForgetPasswordViewModel : BaseViewModel
    {
        public ICommand CancelCM { get; set; }
        public ICommand SendPassCM { get; set; }
        public ForgetPasswordViewModel()
        {
            CancelCM = new RelayCommand<object>((p) => { return p == null ? false : true; }, (p) =>
            {
                LoginViewModel.MainFrame.Content = new LoginPageView();
            });


        }

    }
}
