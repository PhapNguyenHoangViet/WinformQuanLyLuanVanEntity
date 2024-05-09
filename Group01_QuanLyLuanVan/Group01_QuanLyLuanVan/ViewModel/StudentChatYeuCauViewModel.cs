using Group01_QuanLyLuanVan.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Group01_QuanLyLuanVan.Model;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;



namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentChatYeuCauViewModel : BaseViewModel, INotifyPropertyChanged
    {

        private int _sliderValue;
        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                if (_sliderValue != value)
                {
                    _sliderValue = value;
                    OnPropertyChanged(nameof(SliderValue));
                }
            }
        }
        public string Quyen { get; set; }

        public ICommand LoadTaskMessageCommand { get; set; }


        public ICommand UpdateTrangThaiCommand { get; set; }

        public ICommand back { get; set; }

        public ObservableCollection<TinNhanYeuCau> Users { get; set; }
        public string Message { get; set; }

        public ICommand AddMsg { get; set; }

        private ObservableCollection<TinNhanYeuCau> _ListMessage;
        public ObservableCollection<TinNhanYeuCau> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<TinNhanYeuCau>()); }
            set { _ListMessage = value; }
        }

        public StudentChatYeuCauViewModel()
        {
            back = new RelayCommand<StudentChatYeuCauView>((p) => true, p => _back(p));

            UpdateTrangThaiCommand = new RelayCommand<StudentChatYeuCauView>((p) => true, (p) => _UpdateTrangThaiCommand(p));
            var messages = DataProvider.Ins.DB.TinNhanYeuCaus.Where(dt => dt.yeuCauId == Const.yeuCauId).ToList();
            foreach (TinNhanYeuCau msg in messages)

            {
                int tinNhanId = int.Parse(msg.tinNhanId.ToString());
                string tinNhan = msg.tinNhan.ToString();
                DateTime thoiGian = DateTime.Parse(msg.thoiGian.ToString());
                string username = msg.username.ToString();
                int yeuCauId = Convert.ToInt32(msg.yeuCauId);
                TaiKhoan tk = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(x => x.username == username);

                string ava = "";
                if (Const.taiKhoan.avatar == "/Resource/Image/addava.png")
                    ava = Const._localLink + "/Resource/Ava/addava.png";
                else
                    ava = Const._localLink + tk.avatar;

                ListMessage.Add(new TinNhanYeuCau(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
            }

            Users = new ObservableCollection<TinNhanYeuCau>();
            ListMessage = new ObservableCollection<TinNhanYeuCau>();
            Const._server.connectedEvent += UserConnected;
            Const._server.msgReceivedEvent += MessageReceived;
            Const._server.userDisconnectEvent += UserDisconnected;

            AddMsg = new RelayCommand<StudentChatYeuCauView>((p) => true, (p) => _AddMsg(p));
            LoadTaskMessageCommand = new RelayCommand<StudentChatYeuCauView>((p) => true, (p) => _LoadTaskMessageCommand(p));

        }
        void _LoadTaskMessageCommand(StudentChatYeuCauView msgView)
        {
            msgView.TenTask.Text = Const.YeuCau.noiDung;
            msgView.ListMessageView.ItemsSource = listMsg();

        }
        private void UserDisconnected()
        {
            var uid = Const._server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.uid == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));

        }

        private void MessageReceived()
        {
            var msg = Const._server.PacketReader.ReadMessage();
            string[] splittedStrings = msg.Split(new string[] { "|" }, StringSplitOptions.None);
            string message = splittedStrings[0].ToString();
            int yeucauId = int.Parse(splittedStrings[1].ToString());

            //messageTaskDAO.AddMessage(message, DateTime.Now, Const.sinhVien.Username, yeucauId);
            var messages = DataProvider.Ins.DB.TinNhanYeuCaus.Where(dt => dt.yeuCauId == Const.yeuCauId).ToList();

            if (messages.Count > 0)
            {
                var lastMsg = messages.LastOrDefault();
                if (lastMsg != null)
                {
                    int tinNhanId = int.Parse(lastMsg.tinNhanId.ToString());
                    string tinNhan = lastMsg.tinNhan.ToString();
                    DateTime thoiGian = DateTime.Parse(lastMsg.thoiGian.ToString());
                    string username = lastMsg.username.ToString();
                    int yeuCauId = Convert.ToInt32(lastMsg.yeuCauId);
                    TaiKhoan tk = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(x => x.username == username);

                    string ava = "";
                    if (Const.taiKhoan.avatar == "/Resource/Image/addava.png")
                        ava = Const._localLink + "/Resource/Ava/addava.png";
                    else
                        ava = Const._localLink + tk.avatar;


                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ListMessage.Add(new TinNhanYeuCau(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));

                    });
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                StudentChatYeuCauView studentChatYeuCauView = new StudentChatYeuCauView();
                studentChatYeuCauView.TenTask.Text = Const.YeuCau.noiDung;
                studentChatYeuCauView.ListMessageView.ItemsSource = listMsg();
                studentChatYeuCauView.ListMessageView.Items.Refresh();
                StudentMainViewModel.MainFrame.Content = studentChatYeuCauView;
            });
        }

        private void UserConnected()
        {
            var user = new TinNhanYeuCau
                (Const._server.PacketReader.ReadMessage(),
                Const._server.PacketReader.ReadMessage());

            if (!Users.Any(x => x.uid == user.uid))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));

            }
        }

        void _AddMsg(StudentChatYeuCauView p)
        {
            if (p.Msg.Text == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung.");
                return;
            }
            else
            {
                Quyen = "0";
                TinNhanYeuCau msg = new TinNhanYeuCau
                {
                    tinNhan = p.Msg.Text,
                    thoiGian = DateTime.Now,
                    username = Const.sinhVien.username,
                    yeuCauId = Const.yeuCauId
                };
                DataProvider.Ins.DB.TinNhanYeuCaus.Add(msg);
                DataProvider.Ins.DB.SaveChanges();
                Const._server.SendMessageToServer(Message + "|" + Const.yeuCauId.ToString());
                p.Msg.Text = "";

            }

        }
        ObservableCollection<TinNhanYeuCau> listMsg()
        {
            ListMessage = new ObservableCollection<TinNhanYeuCau>();
            var messages = DataProvider.Ins.DB.TinNhanYeuCaus.Where(dt => dt.yeuCauId == Const.yeuCauId).ToList();
            foreach (TinNhanYeuCau msg in messages)

            {
                int tinNhanId = int.Parse(msg.tinNhanId.ToString());
                string tinNhan = msg.tinNhan.ToString();
                DateTime thoiGian = DateTime.Parse(msg.thoiGian.ToString());
                string username = msg.username.ToString();
                int yeuCauId = Convert.ToInt32(msg.yeuCauId);
                TaiKhoan tk = DataProvider.Ins.DB.TaiKhoans.FirstOrDefault(x => x.username == username);

                string ava = "";
                if (Const.taiKhoan.avatar == "/Resource/Image/addava.png")
                    ava = Const._localLink + "/Resource/Ava/addava.png";
                else
                    ava = Const._localLink + tk.avatar;

                ListMessage.Add(new TinNhanYeuCau(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
            }
            return ListMessage;
        }


        void _UpdateTrangThaiCommand(StudentChatYeuCauView p)
        {
            int trangThai = SliderValue;
            try
            {
                YeuCau yc = DataProvider.Ins.DB.YeuCaus.FirstOrDefault(x => x.yeuCauId == Const.yeuCauId);
                if (yc != null)
                {
                    yc.trangThai = trangThai;
                    DataProvider.Ins.DB.SaveChanges();
                }

                MessageBox.Show("Cập nhật thành công!");
                StudentUpdateTaskView taskView = new StudentUpdateTaskView();
                StudentMainViewModel.MainFrame.Content = taskView;
            }
            catch
            {
                MessageBox.Show("Cập nhật không thành công!");
            }
        }

        void _back(StudentChatYeuCauView paramater)
        {
            StudentUpdateTaskView taskView = new StudentUpdateTaskView();
            StudentMainViewModel.MainFrame.Content = taskView;
        }
    }
}
