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
    public class TeacherNotiViewModel:BaseViewModel
    {
        private ObservableCollection<DeTai> _ListTopic;
        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTopicsCommand { get; set; }
        public ICommand DetailTopicsCommand { get; set; }
        public ICommand LoadListTopicCommand { get; set; }
        public ObservableCollection<DeTai> Topics { get; set; }

        public ObservableCollection<ThongBao> Notis { get; set; }

        public TeacherNotiViewModel()
        {
            Topics = new ObservableCollection<DeTai>();
            var topicsData = DataProvider.Ins.DB.DeTais.Where(dt => dt.giangVienId == Const.giangVien.giangVienId).ToList();

            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;
                string moTa = dt.moTa;
                string yeuCauChung = dt.yeuCauChung;
                DateTime ngayBatDau = Convert.ToDateTime(dt.ngayBatDau);
                DateTime ngayKetThuc;
                try
                {
                    ngayKetThuc = Convert.ToDateTime(dt.ngayKetThuc);
                }
                catch
                {
                    ngayKetThuc = Convert.ToDateTime(dt.ngayBatDau);
                }
                int soLuong = Convert.ToInt32(dt.soLuong);
                int trangThai = Convert.ToInt32(dt.trangThai);
                int an = Convert.ToInt32(dt.an);
                string tenTrangThai = "";
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
                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đăng ký";
                }
                else
                {
                    tenTrangThai = "Chưa đăng ký";
                }
                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom));
            }
            ListTopic = Topics;
            ListTK = new ObservableCollection<string>() { "Đề tài", "Thể loại", "Nhóm" };
            DetailTopicsCommand = new RelayCommand<TeacherNotiView>((p) => { return p.ListTopicView.SelectedItem == null ? false : true; }, (p) => _DetailTopicsCommand(p));
            SearchTopicsCommand = new RelayCommand<TeacherNotiView>((p) => { return p == null ? false : true; }, (p) => _SearchTopicsCommand(p));
            LoadListTopicCommand = new RelayCommand<TeacherNotiView>((p) => true, (p) => _LoadListTopicCommand(p));
        }
        void _LoadListTopicCommand(TeacherNotiView topicsView)
        {
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            topicsView.cbxChon.SelectedIndex = 0;
        }

        void _DetailTopicsCommand(TeacherNotiView topicsView)
        {
            // chuyển sang view detail topic
            TeacherNotiDetailView notiView = new TeacherNotiDetailView();
            DeTai temp = (DeTai)topicsView.ListTopicView.SelectedItem;
            notiView.TenDeTai.Text = temp.tenDeTai;
            Const.deTaiId = temp.deTaiId;
            Notis = new ObservableCollection<ThongBao>();
            var thongBaosData = DataProvider.Ins.DB.ThongBaos
                        .Where(tb => tb.deTaiId == Const.deTaiId)
                        .ToList();
            foreach (ThongBao tb in thongBaosData)
            {
                int thongBaoId = Convert.ToInt32(tb.thongBaoId);
                string tieuDe = (tb.tieude).ToString();
                string noiDung = (tb.noiDung).ToString();
                string deTaiId = (tb.deTaiId).ToString();
                DateTime ngay = Convert.ToDateTime(tb.ngay);

                Notis.Add(new ThongBao(thongBaoId, tieuDe, noiDung, deTaiId, ngay));
            }
            notiView.ListThongBaoView.ItemsSource = Notis;
            notiView.ListThongBaoView.SelectedItem = null;
            TeacherMainViewModel.MainFrame.Content = notiView;
        }
        void _SearchTopicsCommand(TeacherNotiView topicsView)
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
                                if ((s.tenNhom.ToString()).ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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
                string moTa = dt.moTa;
                string yeuCauChung = dt.yeuCauChung;
                DateTime ngayBatDau = Convert.ToDateTime(dt.ngayBatDau);
                DateTime ngayKetThuc;
                try
                {
                    ngayKetThuc = Convert.ToDateTime(dt.ngayKetThuc);
                }
                catch
                {
                    ngayKetThuc = Convert.ToDateTime(dt.ngayBatDau);
                }
                int soLuong = Convert.ToInt32(dt.soLuong);
                int trangThai = Convert.ToInt32(dt.trangThai);
                int an = Convert.ToInt32(dt.an);
                string tenTrangThai = "";
                int nhomId = -1;
                var dt1 = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == deTaiId);
                if (dt != null)
                {
                    if (dt.nhomId != null)
                    {
                        nhomId = (int)dt.nhomId;
                    }
                }
                string tenNhom = "Nhóm " + nhomId.ToString();
                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đăng ký";
                }
                else
                {
                    tenTrangThai = "Chưa đăng ký";
                }
                if (an != 1 && nhomId != -1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, tenNhom));
            }
            return Topics;
        }
    }
}
