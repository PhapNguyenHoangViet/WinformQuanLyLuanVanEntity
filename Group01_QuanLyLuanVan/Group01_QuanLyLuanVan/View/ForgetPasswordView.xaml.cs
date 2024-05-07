using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using Group01_QuanLyLuanVan.DAO;

namespace Group01_QuanLyLuanVan.View
{
    /// <summary>
    /// Interaction logic for ForgetPasswordView.xaml
    /// </summary>
    public partial class ForgetPasswordView : Page
    {
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();
        public ForgetPasswordView()
        {
            InitializeComponent();
        }

        private void sendmailbtn_Click(object sender, RoutedEventArgs e)
        {
            TaiKhoan tk = tkDAO.FindOneByMail(MailAddress.Text);
            if (tk == null)
            {
                MessageBox.Show("Email này chưa được đăng ký !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string nd = "Vui lòng nhập mật khẩu " + tk.Password + " để đăng nhập. Trân trọng !";
            MailMessage message = new MailMessage("21110587@student.hcmute.edu.vn", MailAddress.Text, "Lấy lại mật khẩu", nd);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("21110587@student.hcmute.edu.vn", "ngahungA@963");
            smtpClient.Send(message);
            MessageBox.Show("Đã gửi mật khẩu vào Email đăng ký !", "Thông báo");
        }
    }
}
