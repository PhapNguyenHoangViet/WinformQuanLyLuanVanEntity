using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using Group01_QuanLyLuanVan.DAO;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class VerifyCodeViewModel : BaseViewModel
    {

        TaiKhoanDAO tkDAO = new TaiKhoanDAO();
        public ICommand ConfirmCommand { get; set; }

        public VerifyCodeViewModel()
        {
            ConfirmCommand = new RelayCommand<VerifyCodeView>((p) => true, (p) => Confirm(p));
        }

        private void Confirm(VerifyCodeView parameter)
        {
            string verificationCode = parameter.Digit1.Text + parameter.Digit2.Text + parameter.Digit3.Text + parameter.Digit4.Text + parameter.Digit5.Text + parameter.Digit6.Text;
            //string savedVerificationCode = Const.taiKhoan.Code;
            string savedVerificationCode = "123456";
            if (verificationCode == savedVerificationCode)
            {
                tkDAO.UpdateStatus(Const.taiKhoan.Username);
                Window oldWindow = App.Current.MainWindow;
                oldWindow.Close();
            }
            else
            {
                MessageBox.Show("Mã xác nhận không đúng. Vui lòng nhập lại.");
                ClearTextBoxes(parameter);
            }
        }

        private void ClearTextBoxes(VerifyCodeView parameter)
        {
            parameter.Digit1.Clear();
            parameter.Digit2.Clear();
            parameter.Digit3.Clear();
            parameter.Digit4.Clear();
            parameter.Digit5.Clear();
            parameter.Digit6.Clear();
        }
    }
}
