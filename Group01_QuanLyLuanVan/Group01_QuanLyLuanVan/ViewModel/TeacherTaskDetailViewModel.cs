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
        public ICommand AddTask { get; set; }


        private ObservableCollection<YeuCau> _ListTask;
        public ObservableCollection<YeuCau> ListTask
        {
            get { return _ListTask ?? (_ListTask = new ObservableCollection<YeuCau>()); }
            set { _ListTask = value; }
        }
        private ObservableCollection<TinNhanYeuCau> _ListMessage;
        public ObservableCollection<TinNhanYeuCau> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<TinNhanYeuCau>()); }
            set { _ListMessage = value; }
        }
        public ICommand back { get; set; }
        public ICommand LoadTasksCommand { get; set; }
        public ICommand MessageTaskCommand { get; set; }

        public ObservableCollection<TinNhanYeuCau> MessageTasks { get; set; }

        public TeacherTaskDetailViewModel()
        {
            back = new RelayCommand<TeacherTaskDetailView>((p) => true, p => _back(p));

            AddTask = new RelayCommand<TeacherTaskDetailView>((p) => true, (p) => _AddTask(p));
            MessageTaskCommand = new RelayCommand<TeacherTaskDetailView>((p) => { return p.ListTaskView.SelectedItem == null ? false : true; }, (p) => _MessageTaskCommand(p));
            var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(dt => dt.deTaiId == Const.deTaiId).ToList();

            foreach (YeuCau yc in tasksdata)
            {
                int yeuCauId = int.Parse(yc.yeuCauId.ToString());
                string noiDung = yc.noiDung.ToString();
                string deTaiId = yc.deTaiId.ToString();
                int trangThai = Convert.ToInt32(yc.trangThai);
                ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }

            LoadTasksCommand = new RelayCommand<TeacherTaskDetailView>((p) => true, (p) => _LoadTasksCommand(p));

        }

        void _MessageTaskCommand(TeacherTaskDetailView teacherTaskDetailView)
        {


            TeacherTaskMessageView messageView = new TeacherTaskMessageView();
            YeuCau temp = (YeuCau)teacherTaskDetailView.ListTaskView.SelectedItem;

            Const.yeuCauId = temp.yeuCauId;
            Const.YeuCau = temp;
            //messageView.TenDeTai.Text = teacherTaskDetailView.TenDeTai.Text;
            messageView.TenTask.Text = temp.noiDung.ToString();
            messageView.TienDo.Text = temp.trangThai.ToString();
            MessageTasks = new ObservableCollection<TinNhanYeuCau>();
            var messages = DataProvider.Ins.DB.TinNhanYeuCaus.Where(dt => dt.yeuCauId == temp.yeuCauId).ToList();
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

                MessageTasks.Add(new TinNhanYeuCau(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
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
                YeuCau yc = new YeuCau(p.TaskName.Text, 0, Const.deTaiId);

                DataProvider.Ins.DB.YeuCaus.Add(yc);
                DataProvider.Ins.DB.SaveChanges();

                var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(dt => dt.deTaiId == Const.deTaiId).ToList();
                if (tasksdata.Count > 0)
                {
                    var lastTask = tasksdata.LastOrDefault();
                    if (lastTask != null)
                    {
                        int trangThai = 0;
                        if (lastTask.trangThai == 1)
                        {
                            trangThai = 1;
                        }
                        string noiDung = lastTask.noiDung;
                        string deTaiId = lastTask.deTaiId;
                        int yeuCauId = lastTask.yeuCauId;

                        ListTask.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
                    }
                }
                p.TaskName.Text = "";

                TeacherTaskDetailView topicsView = new TeacherTaskDetailView();
                topicsView.TenDeTai.Text = Const.DeTai.tenDeTai;
                topicsView.ListTaskView.ItemsSource = listTask();
                topicsView.ListTaskView.Items.Refresh();
                TeacherMainViewModel.MainFrame.Content = topicsView;
            }
        }

        void _LoadTasksCommand(TeacherTaskDetailView tasksView)
        {
            tasksView.TenDeTai.Text = Const.DeTai.tenDeTai;
            tasksView.ListTaskView.ItemsSource = listTask();

        }
        ObservableCollection<YeuCau> listTask()
        {
            ListTask = new ObservableCollection<YeuCau>();
            var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(dt => dt.deTaiId == Const.deTaiId).ToList();

            foreach (YeuCau yc in tasksdata)
            {
                int yeuCauId = int.Parse(yc.yeuCauId.ToString());
                string noiDung = yc.noiDung.ToString();
                string deTaiId = yc.deTaiId.ToString();
                int trangThai = Convert.ToInt32(yc.trangThai);
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
