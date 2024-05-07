using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Group01_QuanLyLuanVan.View;
using System.IO;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentMailViewModel:BaseViewModel
    {
        List<string> file_list;
        string[] files = new string[0];
        public ICommand SendMSG { get; set; }
        public ICommand SendAttachment { get; set; }
        public StudentMailViewModel()
        {
            SendAttachment = new RelayCommand<StudentMailView>((p) => true, (p) => _SendAttachment(p));
            SendMSG = new RelayCommand<StudentMailView>((p) => true, (p) => _SendMSG(p));
        }
        void _SendAttachment(StudentMailView parameter)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Title = "Select attached files";
                file.Multiselect = true;
                file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.docx";
                file.RestoreDirectory = true;
                if (file.ShowDialog() == true)
                {
                    file_list = new List<string>();
                    foreach (var item in file.FileNames)
                    {
                        file_list.Add(item);
                        if (!File.Exists(item))
                        {
                            MessageBox.Show("File does not exist! ", "Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                }


                if (file_list.Count() == null)
                {
                    files = file_list.ToArray();
                }
                else
                {
                    files = file_list.ToArray();
                }
                int filenum = file.FileNames.Count();
                parameter.attachButton.Content = "Attachments(" + filenum + ")";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        void _SendMSG(StudentMailView parameter)
        {
            MailMessage message = new MailMessage();
            Attachment attachment;
            string msg = parameter.MSGBox.Text;
            message.From = new MailAddress("21110587@student.hcmute.edu.vn");
            message.To.Add(parameter.EmailAddress.Text);
            message.Subject = parameter.SubjectBox.Text;
            message.Body = msg;
            message.IsBodyHtml = true;
            foreach (var item in files)
            {

                attachment = new System.Net.Mail.Attachment(item);
                message.Attachments.Add(attachment);
            }
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential("21110587@student.hcmute.edu.vn", "ngahungA@963");
            smtpClient.Send(message);
            MessageBox.Show("Đã gửi thành công!", "Thông báo");
        }
    }
}
