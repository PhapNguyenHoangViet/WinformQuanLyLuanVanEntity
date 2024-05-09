using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var topicsData = DataProvider.Ins.DB.DeTais.Where(dt => dt.giangVienId == Const.giangVien.giangVienId).ToList();

            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;
                int an = Convert.ToInt32(dt.an);

                int nhomId = -1;
                var dt1 = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == deTaiId);
                if (dt1 != null)
                {
                    if (dt1.nhomId != null)
                    {
                        nhomId = (int)dt1.nhomId;
                    }
                }
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
            taskView.TenDeTai.Text = temp.tenDeTai;
            Const.deTaiId =  temp.deTaiId;
            Tasks = new ObservableCollection<YeuCau>();
            var tasksdata = DataProvider.Ins.DB.YeuCaus.Where(dt => dt.deTaiId == temp.deTaiId).ToList();
            foreach (YeuCau yc in tasksdata)
            {
                int yeuCauId = int.Parse(yc.yeuCauId.ToString());
                string noiDung =yc.noiDung.ToString();
                string deTaiId = yc.deTaiId.ToString();
                int trangThai = Convert.ToInt32(yc.trangThai);
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
                                if (s.tenDeTai.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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
                                if (s.tenTheLoai.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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
                                if (s.tenNhom.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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
            var topicsData = DataProvider.Ins.DB.DeTais.Where(dt => dt.giangVienId == Const.giangVien.giangVienId).ToList();

            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;
                int an = Convert.ToInt32(dt.an);

                int nhomId = -1;
                var dt1 = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == deTaiId);
                if (dt1 != null)
                {
                    if (dt1.nhomId != null)
                    {
                        nhomId = (int)dt1.nhomId;
                    }
                }
                string tenNhom = "Nhóm " + nhomId.ToString();

                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom));
            }
            return Topics;
        }
    }
}
