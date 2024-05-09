using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentListTopicViewModel : BaseViewModel
    {

        private ObservableCollection<DeTai> _ListTopic;
        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value;/* OnPropertyChanged();*/ } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTopicsCommand { get; set; }
        public ICommand DetailTopicsCommand { get; set; }
        public ObservableCollection<DeTai> Topics { get; set; }
        public ICommand LoadTopicsCommand { get; set; }
        public ICommand AddTopicsCommand { get; set; }
        public StudentListTopicViewModel()
        {

            Topics = new ObservableCollection<DeTai>();

            var topicsData = DataProvider.Ins.DB.DeTais
            .Where(dt => dt.GiangVien.khoaId == Const.sinhVien.khoaId && dt.trangThai != 2 && dt.an != 1)
            .ToList();

            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;
                string hoTen = dt.GiangVien.hoTen;
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

                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đăng ký";
                }
                else if (trangThai == 0)
                {
                    tenTrangThai = "Chưa đăng ký";
                }
                else
                {
                    tenTrangThai = "Đề xuất";
                }
                if (an != 1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, hoTen, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, soLuong, tenTrangThai));
            }
            ListTopic = Topics;
            ListTK = new ObservableCollection<string>() { "Đề tài", "Thể loại", "Giảng viên" };
            DetailTopicsCommand = new RelayCommand<StudentListTopicView>((p) => { return p.ListTopicView.SelectedItem == null ? false : true; }, (p) => _DetailTopicsCommand(p));
            SearchTopicsCommand = new RelayCommand<StudentListTopicView>((p) => { return p == null ? false : true; }, (p) => _SearchTopicsCommand(p));
            LoadTopicsCommand = new RelayCommand<StudentListTopicView>((p) => true, (p) => _LoadTopicsCommand(p));
            AddTopicsCommand = new RelayCommand<StudentListTopicView>((p) => true, (p) => _AddTopicsCommand(p));
        }
        void _AddTopicsCommand(StudentListTopicView topicsView)
        {
            var nhomId = DataProvider.Ins.DB.SinhViens
                .Where(sv => sv.username == Const.taiKhoan.username)
                .Select(sv => sv.nhomId)
                .FirstOrDefault();
            var trangThai = DataProvider.Ins.DB.SinhViens
                .Where(sv => sv.username == Const.taiKhoan.username)
                .Join(DataProvider.Ins.DB.DeTais, sv => sv.nhomId, dt => dt.nhomId, (sv, dt) => dt.trangThai)
                .FirstOrDefault();
            int TrangThaiResult = trangThai != null ? Convert.ToInt32(trangThai) : 0;
            if (nhomId != null && TrangThaiResult != 2)
            {
                MessageBox.Show("Bạn đã đăng ký đề tài trước đó rồi!");
            }
            else if (nhomId != null && TrangThaiResult == 2)
            {
                MessageBox.Show("Bạn đã đề xuất đề tài rồi!");
            }
            else
            {
                StudentAddTopicsView addTopicsView = new StudentAddTopicsView();
                StudentMainViewModel.MainFrame.Content = addTopicsView;
            }

        }
        void _LoadTopicsCommand(StudentListTopicView topicsView)
        {
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.cbxChon.SelectedIndex = 0;
        }
        ObservableCollection<DeTai> listTopic()
        {
            Topics = new ObservableCollection<DeTai>();

            var topicsData = DataProvider.Ins.DB.DeTais
            .Where(dt => dt.GiangVien.khoaId == Const.sinhVien.khoaId && dt.trangThai != 2 && dt.an != 1)
            .ToList();

            foreach (DeTai dt in topicsData)
            {
                string deTaiId = dt.deTaiId;
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;
                string hoTen = dt.GiangVien.hoTen;
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

                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đăng ký";
                }
                else if (trangThai == 0)
                {
                    tenTrangThai = "Chưa đăng ký";
                }
                else
                {
                    tenTrangThai = "Đề xuất";
                }
                if (an != 1)
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, hoTen, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, soLuong, tenTrangThai));
            }
            return Topics;
        }
        void _DetailTopicsCommand(StudentListTopicView topicsView)
        {
            StudentRegisterTopicView detailTopic = new StudentRegisterTopicView();
            DeTai temp = (DeTai)topicsView.ListTopicView.SelectedItem;
            detailTopic.deTaiId.Text = temp.deTaiId;
            detailTopic.TenDeTai.Text = temp.tenDeTai;
            detailTopic.TenTheLoai.Text = temp.tenTheLoai;
            detailTopic.HoTen.Text = temp.hoTen;
            detailTopic.MoTa.Text = temp.moTa;
            detailTopic.YeuCau.Text = temp.yeuCauChung;
            detailTopic.NgayBatDau.Text = temp.ngayBatDau.ToString();
            if (temp.ngayBatDau.ToString() == temp.ngayKetThuc.ToString())
                detailTopic.NgayKetThuc.Text = "";
            else
                detailTopic.NgayKetThuc.Text = temp.ngayKetThuc.ToString();
            detailTopic.SoLuong.Text = temp.soLuong.ToString();
            detailTopic.TenTrangThai.Text = temp.tenTrangThai.ToString();

            ListTopic = Topics;
            topicsView.ListTopicView.ItemsSource = ListTopic;
            topicsView.ListTopicView.SelectedItem = null;
            StudentMainViewModel.MainFrame.Content = detailTopic;
        }

        void _SearchTopicsCommand(StudentListTopicView topicsView)
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
                                if (s.hoTen.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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



    }
}