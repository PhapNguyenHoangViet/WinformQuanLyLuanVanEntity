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

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherScoreViewModel : BaseViewModel 
    {
        private ObservableCollection<DeTai> _ListTopic;
        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTopicsCommand { get; set; }
        public ICommand DetailTopicsCommand { get; set; }
        public ICommand LoadListTopicCommand { get; set; }
        public ObservableCollection<DeTai> Topics { get; set; }

        public ObservableCollection<YeuCau> Tasks { get; set; }

        public TeacherScoreViewModel()
        {
            Topics = new ObservableCollection<DeTai>();
            var topicsData = DataProvider.Ins.DB.DeTais
                                    .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.an != 1);
            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.tenTheLoai;
                int an = Convert.ToInt32((dt.an).ToString());

                float diem = 0;
                diem = float.Parse((dt.diem).ToString());
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

                string score = "";
                
                if (diem == 0)
                    score = "Chưa chấm";
                else score = diem.ToString();

                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom, score));

            }
            ListTopic = Topics;
            ListTK = new ObservableCollection<string>() { "Đề tài", "Thể loại", "Nhóm" };
            SearchTopicsCommand = new RelayCommand<TeacherScoreView>((p) => { return p == null ? false : true; }, (p) => _SearchTopicsCommand(p));
            LoadListTopicCommand = new RelayCommand<TeacherScoreView>((p) => true, (p) => _LoadListTopicCommand(p));
            DetailTopicsCommand = new RelayCommand<TeacherScoreView>((p) => { return p.ListTopicView.SelectedItem == null ? false : true; }, (p) => _DetailTopicsCommand(p));

        }

        void _DetailTopicsCommand(TeacherScoreView topicsView)
        {
            TeacherScoreDetailView scoreView = new TeacherScoreDetailView();
            DeTai temp = (DeTai)topicsView.ListTopicView.SelectedItem;
            Const.DeTai = temp;
            scoreView.TenDeTai.Text = temp.tenDeTai;
            if (temp.phanTram == "Chưa chấm")
                scoreView.score.Text = "";
            else scoreView.score.Text = temp.phanTram;

            Const.deTaiId = temp.deTaiId;
            
            TeacherMainViewModel.MainFrame.Content = scoreView;
        }


        void _LoadListTopicCommand(TeacherScoreView topicsView)
        {
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            topicsView.cbxChon.SelectedIndex = 0;
        }



        void _SearchTopicsCommand(TeacherScoreView topicsView)
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
            var topicsData = DataProvider.Ins.DB.DeTais
                                    .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.an != 1);
            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.tenTheLoai;
                int an = Convert.ToInt32((dt.an).ToString());

                float diem = 0;
                diem = float.Parse((dt.diem).ToString());
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

                string score = "";

                if (diem == 0)
                    score = "Chưa chấm";
                else score = diem.ToString();

                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom, score));

            }
            return Topics;
        }
    }
}
