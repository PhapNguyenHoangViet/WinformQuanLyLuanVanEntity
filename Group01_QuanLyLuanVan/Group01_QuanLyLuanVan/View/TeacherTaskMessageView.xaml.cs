using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TeacherTaskMessageView.xaml
    /// </summary>
    public partial class TeacherTaskMessageView : Page
    {
        public TeacherTaskMessageView()
        {
            InitializeComponent();
        }
        /*public ObservableCollection<ChatYeuCau> Messages { get; set; }

        public TeacherTaskMessageView()
        {
            InitializeComponent();
            Messages = new ObservableCollection<ChatYeuCau>();
            MessageListView.ItemsSource = Messages;

            // Thêm một số tin nhắn giả định để thử nghiệm
            Messages.Add(new ChatYeuCau
            {
                TinNhanId = 1,
                YeuCauId = 1,
                NguoiGui = "Aliceeeeeee",
                NguoiNhan = "Bob",
                NoiDung = "Xin chào Bob!gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg",
                ThoiGian = DateTime.Now,
                AvatarPath = "avatar_alice.jpg"
            });
            Messages.Add(new ChatYeuCau
            {
                TinNhanId = 1,
                YeuCauId = 1,
                NguoiGui = "Bob",
                NguoiNhan = "Alice",
                NoiDung = "Xin chào Alice!",
                ThoiGian = DateTime.Now.AddMinutes(1),
                AvatarPath = "avatar_bob.jpg"
            });
        }
        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string messageText = MessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(messageText))
            {
                Messages.Add(new ChatYeuCau
                {
                    NguoiGui = "You",
                    NguoiNhan = "Friend",
                    NoiDung = messageText,
                    ThoiGian = DateTime.Now,
                    AvatarPath = "your_avatar.jpg" // Đường dẫn tới avatar của bạn
                });

                // Xóa nội dung của TextBox sau khi gửi
                MessageTextBox.Text = "";
            }
        }*/

    }
}

