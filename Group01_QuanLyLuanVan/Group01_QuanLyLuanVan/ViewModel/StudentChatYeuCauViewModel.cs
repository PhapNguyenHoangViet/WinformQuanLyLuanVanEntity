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
using Group01_QuanLyLuanVan.DAO;
using System.Collections.ObjectModel;
using System.Data;



namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentChatYeuCauViewModel : BaseViewModel, INotifyPropertyChanged
    {
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();

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

        public ObservableCollection<MessageTask> Users { get; set; }
        public string Message { get; set; }

        MessageTaskDAO messageTaskDAO = new MessageTaskDAO();

        public ICommand AddMsg { get; set; }

        private ObservableCollection<MessageTask> _ListMessage;
        public ObservableCollection<MessageTask> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<MessageTask>()); }
            set { _ListMessage = value; }
        }

        public StudentChatYeuCauViewModel()
        {
            back = new RelayCommand<StudentChatYeuCauView>((p) => true, p => _back(p));

            UpdateTrangThaiCommand = new RelayCommand<StudentChatYeuCauView>((p) => true, (p) => _UpdateTrangThaiCommand(p));
            var msgsdata = messageTaskDAO.LoadListMessageTask(Const.yeuCauId);
            foreach (DataRow row in msgsdata.Rows)
            {
                int tinNhanId = int.Parse(row["tinNhanId"].ToString());
                string tinNhan = row["tinNhan"].ToString();
                DateTime thoiGian = DateTime.Parse(row["thoiGian"].ToString());
                string username = row["username"].ToString();
                int yeuCauId = Convert.ToInt32(row["yeuCauId"]);
                TaiKhoan tk = new TaiKhoan();
                tk = tkDAO.FindOneByUsername(username);
                string ava = Const._localLink + tk.Avatar;

                ListMessage.Add(new MessageTask(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));

            }

            Users = new ObservableCollection<MessageTask>();
            ListMessage = new ObservableCollection<MessageTask>();
            Const._server.connectedEvent += UserConnected;
            Const._server.msgReceivedEvent += MessageReceived;
            Const._server.userDisconnectEvent += UserDisconnected;

            AddMsg = new RelayCommand<StudentChatYeuCauView>((p) => true, (p) => _AddMsg(p));
            LoadTaskMessageCommand = new RelayCommand<StudentChatYeuCauView>((p) => true, (p) => _LoadTaskMessageCommand(p));

        }
        void _LoadTaskMessageCommand(StudentChatYeuCauView msgView)
        {
            msgView.TenTask.Text = Const.YeuCau.NoiDung;
            msgView.ListMessageView.ItemsSource = listMsg();

        }
        private void UserDisconnected()
        {
            var uid = Const._server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));

        }

        private void MessageReceived()
        {
            Quyen = "0";
            var msg = Const._server.PacketReader.ReadMessage();
            string[] splittedStrings = msg.Split(new string[] { "|" }, StringSplitOptions.None);
            string message = splittedStrings[0].ToString();
            int yeucauId = int.Parse(splittedStrings[1].ToString());

            //messageTaskDAO.AddMessage(message, DateTime.Now, Const.sinhVien.Username, yeucauId);
            DataTable dataTable = messageTaskDAO.LoadListMessageTask(yeucauId);
            if (dataTable.Rows.Count > 0)
            {
                DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];
                int tinNhanId = int.Parse(lastRow["tinNhanId"].ToString());
                string tinNhan = lastRow["tinNhan"].ToString();
                DateTime thoiGian = DateTime.Parse(lastRow["thoiGian"].ToString());
                string username = lastRow["username"].ToString();
                int yeuCauId = Convert.ToInt32(lastRow["yeuCauId"]);
                TaiKhoan tk = new TaiKhoan();
                tk = tkDAO.FindOneByUsername(username);
                string ava = Const._localLink + tk.Avatar;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ListMessage.Add(new MessageTask(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
                });
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                StudentChatYeuCauView studentChatYeuCauView = new StudentChatYeuCauView();
                studentChatYeuCauView.TenTask.Text = Const.YeuCau.NoiDung;
                studentChatYeuCauView.ListMessageView.ItemsSource = listMsg();
                studentChatYeuCauView.ListMessageView.Items.Refresh();
                StudentMainViewModel.MainFrame.Content = studentChatYeuCauView;
            });
        }

        private void UserConnected()
        {
            var user = new MessageTask
                (Const._server.PacketReader.ReadMessage(),
                Const._server.PacketReader.ReadMessage());

            if (!Users.Any(x => x.UID == user.UID))
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
                messageTaskDAO.AddMessage(p.Msg.Text, DateTime.Now, Const.sinhVien.Username, Const.yeuCauId);
                Const._server.SendMessageToServer(Message + "|" + Const.yeuCauId.ToString());


                //DataTable dataTable = messageTaskDAO.LoadListMessageTask(Const.yeuCauId);
                //if (dataTable.Rows.Count > 0)
                //{
                //    DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];
                //    int tinNhanId = int.Parse(lastRow["tinNhanId"].ToString());
                //    string tinNhan = lastRow["tinNhan"].ToString();
                //    DateTime thoiGian = DateTime.Parse(lastRow["thoiGian"].ToString());
                //    string username = lastRow["username"].ToString();
                //    int yeuCauId = Convert.ToInt32(lastRow["yeuCauId"]);
                //    ListMessage.Add(new MessageTask(tinNhanId, tinNhan, thoiGian, username, yeuCauId));
                //}
                p.Msg.Text = "";

                //StudentChatYeuCauView studentChatYeuCauView = new StudentChatYeuCauView();
                //studentChatYeuCauView.TenTask.Text = Const.YeuCau.NoiDung;
                //studentChatYeuCauView.ListMessageView.ItemsSource = listMsg();
                //studentChatYeuCauView.ListMessageView.Items.Refresh();
                //StudentMainViewModel.MainFrame.Content = studentChatYeuCauView;

            }

        }
        ObservableCollection<MessageTask> listMsg()
        {
            ListMessage = new ObservableCollection<MessageTask>();
            DataTable messages = messageTaskDAO.LoadListMessageTask(Const.yeuCauId);
            foreach (DataRow row in messages.Rows)
            {
                int tinNhanId = int.Parse(row["tinNhanId"].ToString());
                string tinNhan = row["tinNhan"].ToString();
                DateTime thoiGian = DateTime.Parse(row["thoiGian"].ToString());
                string username = row["username"].ToString();
                int yeuCauId = Convert.ToInt32(row["yeuCauId"]);
                TaiKhoan tk = new TaiKhoan();
                tk = tkDAO.FindOneByUsername(username);
                string ava = Const._localLink + tk.Avatar;
                ListMessage.Add(new MessageTask(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
            }
            return ListMessage;
        }


        void _UpdateTrangThaiCommand(StudentChatYeuCauView p)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                conn.Open();
                int trangThai = SliderValue;
                try
                {
                    string updateTrangThaiQuery = "UPDATE YeuCau SET trangThai =  @TrangThai FROM YeuCau yc JOIN DeTai dt ON yc.deTaiId = dt.deTaiId JOIN SinhVien sv ON sv.nhomId = dt.nhomId WHERE sv.username = @user and noiDung = @NoiDung";
                    SqlCommand updateTrangThaiCommand = new SqlCommand(updateTrangThaiQuery, conn);
                    updateTrangThaiCommand.Parameters.AddWithValue("@TrangThai", trangThai);
                    updateTrangThaiCommand.Parameters.AddWithValue("@user", Const.sinhVien.Username);
                    updateTrangThaiCommand.Parameters.AddWithValue("@NoiDung", p.TenTask.Text); // Lấy giá trị từ TextBox TenTask
                    updateTrangThaiCommand.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!");

                }

                catch
                {
                    MessageBox.Show("Cập nhật không thành công!");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        void _back(StudentChatYeuCauView paramater)
        {
            StudentUpdateTaskView taskView = new StudentUpdateTaskView();
            StudentMainViewModel.MainFrame.Content = taskView;
        }
    }
}
