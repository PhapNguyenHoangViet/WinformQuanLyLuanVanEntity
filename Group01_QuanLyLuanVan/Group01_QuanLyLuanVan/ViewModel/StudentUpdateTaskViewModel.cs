using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Input;
using Group01_QuanLyLuanVan.View;
using Microsoft.Win32;
using System.Data.Common;
using System.ComponentModel;
using Group01_QuanLyLuanVan.Chat.Net;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentUpdateTaskViewModel : BaseViewModel
    {
        public ICommand ThemTask { get; set; }

        private ObservableCollection<YeuCau> _ListTask;
        public ObservableCollection<YeuCau> ListTask { get => _ListTask; set { _ListTask = value;/* OnPropertyChanged();*/ } }

        public ObservableCollection<YeuCau> Tasks { get; set; }
        public ICommand LoadTasksCommand { get; set; }
        public ICommand MessageTaskCommand { get; set; }
        private ObservableCollection<TinNhanYeuCau> _ListMessage;
        public ObservableCollection<TinNhanYeuCau> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<TinNhanYeuCau>()); }
            set { _ListMessage = value; }
        }

        public ObservableCollection<TinNhanYeuCau> MessageTasks { get; set; }

        public StudentUpdateTaskViewModel()
        {
            Tasks = new ObservableCollection<YeuCau>();
            DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
            string dtTaiId = dt.deTaiId;
            Const.deTaiId = dtTaiId;
            var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(task => task.deTaiId == dtTaiId).ToList();
            foreach (YeuCau yc in tasksdata)
            {
                int yeuCauId = int.Parse(yc.yeuCauId.ToString());
                string noiDung = yc.noiDung.ToString();
                string deTaiId = yc.deTaiId.ToString();
                int trangThai = Convert.ToInt32(yc.trangThai);
                Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            ListTask = Tasks;
            LoadTasksCommand = new RelayCommand<StudentUpdateTaskView>((p) => true, (p) => _LoadTasksCommand(p));
            ThemTask = new RelayCommand<StudentUpdateTaskView>((p) => true, (p) => _ThemTask(p));
            MessageTaskCommand = new RelayCommand<StudentUpdateTaskView>((p) => { return p.ListTaskView.SelectedItem == null ? false : true; }, (p) => _MessageTaskCommand(p));
        }

        void _MessageTaskCommand(StudentUpdateTaskView studentTaskDetailView)
        {
            StudentChatYeuCauView messageView = new StudentChatYeuCauView();
            YeuCau temp = (YeuCau)studentTaskDetailView.ListTaskView.SelectedItem;

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
            StudentMainViewModel.MainFrame.Content = messageView;
        }

        void _LoadTasksCommand(StudentUpdateTaskView tasksView)
        {
            tasksView.ListTaskView.ItemsSource = listTask();

        }
        ObservableCollection<YeuCau> listTask()
        {
            Tasks = new ObservableCollection<YeuCau>();
            DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
            string dtTaiId = dt.deTaiId;
            var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(task => task.deTaiId == dtTaiId).ToList();
            foreach (YeuCau yc in tasksdata)
            {
                int yeuCauId = int.Parse(yc.yeuCauId.ToString());
                string noiDung = yc.noiDung.ToString();
                string deTaiId = yc.deTaiId.ToString();
                int trangThai = Convert.ToInt32(yc.trangThai);
                Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            return Tasks;
        }

        void _ThemTask(StudentUpdateTaskView p)
        {
            if (p.ThemTask.Text == "")
            {
                MessageBox.Show("Vui lòng nhập nội dung.");
                return;
            }
            else
            {
                YeuCau yc = new YeuCau(p.ThemTask.Text, 0, Const.deTaiId);

                DataProvider.Ins.DB.YeuCaus.Add(yc);
                DataProvider.Ins.DB.SaveChanges();
                Tasks = new ObservableCollection<YeuCau>();
                DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
                string dtTaiId = dt.deTaiId;
                var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(task => task.deTaiId == dtTaiId).ToList();
                foreach (YeuCau task in tasksdata)
                {
                    int yeuCauId = int.Parse(task.yeuCauId.ToString());
                    string noiDung = task.noiDung.ToString();
                    string deTaiId = task.deTaiId.ToString();
                    int trangThai = Convert.ToInt32(task.trangThai);
                    Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
                }

                p.ThemTask.Text = "";

                StudentUpdateTaskView tasksView = new StudentUpdateTaskView();
                tasksView.ListTaskView.ItemsSource = Tasks;
                tasksView.ListTaskView.Items.Refresh();
                StudentMainViewModel.MainFrame.Content = tasksView;
            }
        }
    }
}
