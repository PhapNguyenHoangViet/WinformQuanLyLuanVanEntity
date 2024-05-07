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

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherProgressViewModel : BaseViewModel
    {
        DeTaiDAO dtDAO = new DeTaiDAO();
        YeuCauDAO yeuCauDAO = new YeuCauDAO();
        TienDoDAO tienDoDAO = new TienDoDAO();


        private ObservableCollection<DeTai> _ListTopic;
        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTopicsCommand { get; set; }
        public ICommand DetailTopicsCommand { get; set; }
        public ICommand LoadListTopicCommand { get; set; }
        public ObservableCollection<DeTai> Topics { get; set; }

        public ObservableCollection<YeuCau> Tasks { get; set; }

        public TeacherProgressViewModel()
        {
            Topics = new ObservableCollection<DeTai>();
            var topicsData = dtDAO.LoadListTopic(Const.giangVien.GiangVienId);
            foreach (DataRow row in topicsData.Rows)
            {
                string deTaiId = row["deTaiId"].ToString();
                string tenDeTai = row["tenDeTai"].ToString();
                string tenTheLoai = row["tenTheLoai"].ToString();
                string moTa = row["moTa"].ToString();
                string yeuCauChung = row["yeuCauChung"].ToString();
                DateTime ngayBatDau = Convert.ToDateTime(row["ngayBatDau"]);
                DateTime ngayKetThuc;
                try
                {
                    ngayKetThuc = Convert.ToDateTime(row["ngayKetThuc"]);
                }
                catch
                {
                    ngayKetThuc = Convert.ToDateTime(row["ngayBatDau"]);
                }

                int soLuong = Convert.ToInt32(row["soLuong"]);
                int trangThai = Convert.ToInt32(row["trangThai"]);
                int an = Convert.ToInt32(row["an"]);
                string tenTrangThai = "";
                int nhomId = dtDAO.FindNhomIdByDeTaiId(deTaiId);
                string tenNhom = "Nhóm " + nhomId.ToString();
                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đăng ký";
                }
                else
                {
                    tenTrangThai = "Chưa đăng ký";
                }

                TienDo tienDo = tienDoDAO.FindOneByDeTaiId(deTaiId);
                string phanTram = "0 %";
                if (tienDo != null)
                {
                    phanTram = tienDo.PhanTram.ToString() + " %";
                }
                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom, phanTram));
            }
            ListTopic = Topics;
            ListTK = new ObservableCollection<string>() { "Đề tài", "Thể loại", "Nhóm" };
            DetailTopicsCommand = new RelayCommand<TeacherProgressView>((p) => { return p.ListTopicView.SelectedItem == null ? false : true; }, (p) => _DetailTopicsCommand(p));
            SearchTopicsCommand = new RelayCommand<TeacherProgressView>((p) => { return p == null ? false : true; }, (p) => _SearchTopicsCommand(p));
            LoadListTopicCommand = new RelayCommand<TeacherProgressView>((p) => true, (p) => _LoadListTopicCommand(p));
        }
        void _LoadListTopicCommand(TeacherProgressView topicsView)
        {
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            topicsView.cbxChon.SelectedIndex = 0;
        }
        void _DetailTopicsCommand(TeacherProgressView topicsView)
        {
            // chuyển sang view detail topic
            //TeacherTaskDetailView taskView = new TeacherTaskDetailView();
            //DeTai temp = (DeTai)topicsView.ListTopicView.SelectedItem;
            //taskView.TenDeTai.Text = temp.TenDeTai;
            //Tasks = new ObservableCollection<YeuCau>();
            //var tasksdata = yeuCauDAO.LoadListTask(temp.DeTaiId);
            //foreach (DataRow row in tasksdata.Rows)
            //{
            //    int yeuCauId = int.Parse(row["yeuCauId"].ToString());
            //    string noiDung = row["noiDung"].ToString();
            //    string deTaiId = row["deTaiId"].ToString();
            //    int trangThai = Convert.ToInt32(row["trangThai"]);
            //    Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            //}
            //taskView.ListTaskView.ItemsSource = Tasks;
            //taskView.ListTaskView.SelectedItem = null;
            //TeacherMainViewModel.MainFrame.Content = taskView;
        }

        void _SearchTopicsCommand(TeacherProgressView topicsView)
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
                TienDo tienDo = tienDoDAO.FindOneByDeTaiId(deTaiId);
                string phanTram = "0 %";
                if (tienDo != null) 
                {
                    phanTram = tienDo.PhanTram.ToString() + " %";
                }
                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom, phanTram));
            }
            return Topics;
        }
    }
}
