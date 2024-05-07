using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Data.SqlClient;
using Group01_QuanLyLuanVan.Chat.Net;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherTaskDetailViewModel : BaseViewModel
    {
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();

        YeuCauDAO ycDAO = new YeuCauDAO();
        MessageTaskDAO messageTaskDAO = new MessageTaskDAO();


        public ICommand AddTask { get; set; }


        private ObservableCollection<YeuCau> _ListTask;
        public ObservableCollection<YeuCau> ListTask
        {
            get { return _ListTask ?? (_ListTask = new ObservableCollection<YeuCau>()); }
            set { _ListTask = value; }
        }
        private ObservableCollection<MessageTask> _ListMessage;
        public ObservableCollection<MessageTask> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<MessageTask>()); }
            set { _ListMessage = value; }
        }
        public ICommand back { get; set; }
        public ICommand LoadTasksCommand { get; set; }
        public ICommand MessageTaskCommand { get; set; }

        public ObservableCollection<MessageTask> MessageTasks { get; set; }

        public TeacherTaskDetailViewModel()
        {
            back = new RelayCommand<TeacherTaskDetailView>((p) => true, p => _back(p));

            AddTask = new RelayCommand<TeacherTaskDetailView>((p) => true, (p) => _AddTask(p));
            MessageTaskCommand = new RelayCommand<TeacherTaskDetailView>((p) => { return p.ListTaskView.SelectedItem == null ? false : true; }, (p) => _MessageTaskCommand(p));
            var tasksdata = ycDAO.LoadListTask(Const.deTaiId);
            foreach (DataRow row in tasksdata.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);
                ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            LoadTasksCommand = new RelayCommand<TeacherTaskDetailView>((p) => true, (p) => _LoadTasksCommand(p));

        }

        void _MessageTaskCommand(TeacherTaskDetailView teacherTaskDetailView)
        {


            TeacherTaskMessageView messageView = new TeacherTaskMessageView();
            YeuCau temp = (YeuCau)teacherTaskDetailView.ListTaskView.SelectedItem;

            Const.yeuCauId = temp.YeuCauId;
            Const.YeuCau = temp;
            //messageView.TenDeTai.Text = teacherTaskDetailView.TenDeTai.Text;
            messageView.TenTask.Text = temp.NoiDung.ToString();
            messageView.TienDo.Text = temp.TrangThai.ToString();
            MessageTasks = new ObservableCollection<MessageTask>();
            var messages = messageTaskDAO.LoadListMessageTask(temp.YeuCauId);
            foreach (DataRow row in messages.Rows)
            {
                int tinNhanId = int.Parse(row["tinNhanId"].ToString());
                string tinNhan = row["tinNhan"].ToString();
                DateTime thoiGian = DateTime.Parse(row["thoiGian"].ToString());
                string username = row["username"].ToString();
                int yeuCauId = Convert.ToInt32(row["yeuCauId"]);
                TaiKhoan tk = tkDAO.FindOneByUsername(username);
                string ava = "";
                if (Const.taiKhoan.Avatar == "/Resource/Image/addava.png")
                    ava = Const._localLink + "/Resource/Ava/addava.png";
                else
                    ava = Const._localLink + tk.Avatar;

                MessageTasks.Add(new MessageTask(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
            }

            messageView.ListMessageView.ItemsSource = MessageTasks;
            messageView.ListMessageView.SelectedItem = null;
            TeacherMainViewModel.MainFrame.Content = messageView;
        }
        void _AddTask(TeacherTaskDetailView p)
        {
            if (p.TaskName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung.");
                return;
            }
            else
            {
                ycDAO.AddTask(p.TaskName.Text, 0, Const.deTaiId);
                DataTable dataTable = ycDAO.ListYeuCauByDeTaiId(Const.deTaiId);
                if (dataTable.Rows.Count > 0)
                {
                    DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];
                    int trangThai = 0;
                    if (lastRow["TrangThai"].ToString() == "1")
                    {
                        trangThai = 1;
                    }
                    string noiDung = lastRow["noiDung"].ToString();
                    string deTaiId = lastRow["deTaiId"].ToString();
                    int yeuCauId = int.Parse(lastRow["yeuCauId"].ToString());

                    ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
                }
                p.TaskName.Text = "";

                TeacherTaskDetailView topicsView = new TeacherTaskDetailView();
                topicsView.TenDeTai.Text = Const.DeTai.TenDeTai;
                topicsView.ListTaskView.ItemsSource = listTask();
                topicsView.ListTaskView.Items.Refresh();
                TeacherMainViewModel.MainFrame.Content = topicsView;
            }
        }

        void _LoadTasksCommand(TeacherTaskDetailView tasksView)
        {
            tasksView.TenDeTai.Text = Const.DeTai.TenDeTai;
            tasksView.ListTaskView.ItemsSource = listTask();

        }
        ObservableCollection<YeuCau> listTask()
        {
            ListTask = new ObservableCollection<YeuCau>();
            DataTable dataTable = ycDAO.ListYeuCauByDeTaiId(Const.deTaiId);
            foreach (DataRow row in dataTable.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);
                ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            return ListTask;
        }
        void _back(TeacherTaskDetailView paramater)
        {
            TeacherTaskView taskView = new TeacherTaskView();
            TeacherMainViewModel.MainFrame.Content = taskView;
        }
    }
}
