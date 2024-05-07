using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Group01_QuanLyLuanVan.Chat.Net;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherTaskViewModel : BaseViewModel
    {

        DeTaiDAO dtDAO = new DeTaiDAO();

        YeuCauDAO yeuCauDAO = new YeuCauDAO();

        private ObservableCollection<DeTai> _ListTopic;
        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value;OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTopicsCommand { get; set; }
        public ICommand DetailTopicsCommand { get; set; }
        public ICommand LoadListTopicCommand { get; set; }
        public ObservableCollection<DeTai> Topics { get; set; }

        public ObservableCollection<YeuCau> Tasks { get; set; }

        public TeacherTaskViewModel()
        {
            Topics = new ObservableCollection<DeTai>();
            var topicsData = dtDAO.LoadListTopic(Const.giangVien.GiangVienId);
            foreach (DataRow row in topicsData.Rows)
            {
                string deTaiId = row["deTaiId"].ToString();
                string tenDeTai = row["tenDeTai"].ToString();
                string tenTheLoai = row["tenTheLoai"].ToString();
                int an = Convert.ToInt32(row["an"]);
                int nhomId = dtDAO.FindNhomIdByDeTaiId(deTaiId);
                string tenNhom = "Nhóm " + nhomId.ToString();

                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom));
            }
            ListTopic = Topics;
            ListTK = new ObservableCollection<string>() { "Đề tài", "Thể loại", "Nhóm" };
            DetailTopicsCommand = new RelayCommand<TeacherTaskView>((p) => { return p.ListTopicView.SelectedItem == null ? false : true; }, (p) => _DetailTopicsCommand(p));
            SearchTopicsCommand = new RelayCommand<TeacherTaskView>((p) => { return p == null ? false : true; }, (p) => _SearchTopicsCommand(p));
            LoadListTopicCommand = new RelayCommand<TeacherTaskView>((p) => true, (p) => _LoadListTopicCommand(p));
        }
        void _LoadListTopicCommand(TeacherTaskView topicsView)
        {
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            topicsView.cbxChon.SelectedIndex = 0;
        }
        void _DetailTopicsCommand(TeacherTaskView topicsView)
        {
            // chuyển sang view detail topic
            TeacherTaskDetailView taskView = new TeacherTaskDetailView();
            DeTai temp = (DeTai)topicsView.ListTopicView.SelectedItem;
            Const.DeTai = temp;
            taskView.TenDeTai.Text = temp.TenDeTai;
            Const.deTaiId =  temp.DeTaiId;
            Tasks = new ObservableCollection<YeuCau>();
            var tasksdata = yeuCauDAO.LoadListTask(temp.DeTaiId);
            foreach (DataRow row in tasksdata.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);
                Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            taskView.ListTaskView.ItemsSource = Tasks;
            taskView.ListTaskView.SelectedItem = null;
            TeacherMainViewModel.MainFrame.Content = taskView;
        }

        void _SearchTopicsCommand(TeacherTaskView topicsView)
        {
            ObservableCollection<DeTai> temp = new ObservableCollection<DeTai>();
            if (topicsView.cbxChon.Text != "")
            {
                switch (topicsView.cbxChon.SelectedItem.ToString())
                {
                    case "Đề tài":
                        {
                            foreach (DeTai s in ListTopic)
                            {
                                if (s.TenDeTai.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    case "Thể loại":
                        {
                            foreach (DeTai s in ListTopic)
                            {
                                if (s.TenTheLoai.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (DeTai s in ListTopic)
                            {
                                if (s.TenNhom.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                }
                topicsView.ListTopicView.ItemsSource = temp;
            }
            else
                topicsView.ListTopicView.ItemsSource = ListTopic;
        }
        ObservableCollection<DeTai> listTopic()
        {
            Topics = new ObservableCollection<DeTai>();
            var topicsData = dtDAO.LoadListTopic(Const.giangVien.GiangVienId);
            foreach (DataRow row in topicsData.Rows)
            {
                string deTaiId = row["deTaiId"].ToString();
                string tenDeTai = row["tenDeTai"].ToString();
                string tenTheLoai = row["tenTheLoai"].ToString();
                int an = Convert.ToInt32(row["an"]);
                int nhomId = dtDAO.FindNhomIdByDeTaiId(deTaiId);
                string tenNhom = "Nhóm " + nhomId.ToString(); 

                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId,tenDeTai, tenTheLoai, tenNhom));
            }
            return Topics;
        }
    }
}
