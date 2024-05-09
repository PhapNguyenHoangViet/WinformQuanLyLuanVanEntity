using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Group01_QuanLyLuanVan.Chat.Net;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherTaskMessageViewModel : BaseViewModel, INotifyPropertyChanged
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
        public ICommand UpdateTrangThaiCommand { get; set; }

        public ObservableCollection<TinNhanYeuCau> Users { get; set; }
        public string Message { get; set; }
        public string Quyen { get; set; }

        public ICommand AddMsg { get; set; }

        private ObservableCollection<TinNhanYeuCau> _ListMessage;
        public ObservableCollection<TinNhanYeuCau> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<TinNhanYeuCau>()); }
            set { _ListMessage = value; }
        }

        public ICommand back { get; set; }    
        public ICommand LoadTaskMessageCommand { get; set; }


        public TeacherTaskMessageViewModel()
        {
            
            back = new RelayCommand<TeacherTaskMessageView>((p) => true, p => _back(p));
            UpdateTrangThaiCommand = new RelayCommand<TeacherTaskMessageView>((p) => true, (p) => _UpdateTrangThaiCommand(p));

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
            AddMsg = new RelayCommand<TeacherTaskMessageView>((p) => true, (p) => _AddMsg(p));
            LoadTaskMessageCommand = new RelayCommand<TeacherTaskMessageView>((p) => true, (p) => _LoadTaskMessageCommand(p));

        }

        void _LoadTaskMessageCommand(TeacherTaskMessageView msgView)
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
            string msg = Const._server.PacketReader.ReadMessage().ToString();
            string[] splittedStrings = msg.Split(new string[] { "|" }, StringSplitOptions.None);
            string message = splittedStrings[0].ToString();
            int yeucauId = (int)int.Parse(splittedStrings[1].ToString());

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
                Application.Current.Dispatcher.Invoke(() =>
                {

                    TeacherTaskMessageView teacherTaskMessageView = new TeacherTaskMessageView();
                    teacherTaskMessageView.TenTask.Text = Const.YeuCau.noiDung;
                    teacherTaskMessageView.ListMessageView.ItemsSource = listMsg();
                    teacherTaskMessageView.ListMessageView.Items.Refresh();
                    TeacherMainViewModel.MainFrame.Content = teacherTaskMessageView;
                });

            }

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

        void _AddMsg(TeacherTaskMessageView p)
        {
            if (p.Msg.Text == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung.");
                return;
            }
            else
            {
                Quyen = "1";
                TinNhanYeuCau msg = new TinNhanYeuCau
                {
                    tinNhan = p.Msg.Text,
                    thoiGian = DateTime.Now,
                    username = Const.giangVien.username,
                    yeuCauId = Const.yeuCauId
                };
                DataProvider.Ins.DB.TinNhanYeuCaus.Add(msg);
                DataProvider.Ins.DB.SaveChanges();
                Const._server.SendMessageToServer(Message+ "|" + Const.yeuCauId.ToString());
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

        void _UpdateTrangThaiCommand(TeacherTaskMessageView p)
        {
            int trangThai = SliderValue;

                YeuCau yc = DataProvider.Ins.DB.YeuCaus.FirstOrDefault(x => x.yeuCauId == Const.yeuCauId);
                if (yc != null)
                {
                    yc.trangThai = trangThai;
                    DataProvider.Ins.DB.SaveChanges();
                }

                MessageBox.Show("Cập nhật thành công!");
                TeacherTaskDetailView taskView = new TeacherTaskDetailView();
                TeacherMainViewModel.MainFrame.Content = taskView;

        }

        void _back(TeacherTaskMessageView paramater)
        {
            TeacherTaskDetailView taskView = new TeacherTaskDetailView();
            TeacherMainViewModel.MainFrame.Content = taskView;
        }
    }
}
