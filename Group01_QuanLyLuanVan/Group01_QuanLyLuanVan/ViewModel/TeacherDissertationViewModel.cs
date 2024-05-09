using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherDissertationViewModel : BaseViewModel
    {
        private ObservableCollection<DeTai> _ListTopic;
        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value;OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchTopicsCommand { get; set; }
        public ICommand DetailTopicsCommand { get; set; }
        public ICommand LoadListTopicCommand { get; set; }
        public ICommand AddTopic { get; set; }
        public ObservableCollection<DeTai> Topics { get; set; }

        public TeacherDissertationViewModel()
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
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, soLuong, tenTrangThai));
            }
            ListTopic = Topics;
            ListTK = new ObservableCollection<string>() { "Đề tài", "Thể loại", "Trạng thái" };
            DetailTopicsCommand = new RelayCommand<TeacherDissertationView>((p) => { return p.ListTopicView.SelectedItem == null ? false : true; }, (p) => _DetailTopicsCommand(p));
            SearchTopicsCommand = new RelayCommand<TeacherDissertationView>((p) => { return p == null ? false : true; }, (p) => _SearchTopicsCommand(p));
            LoadListTopicCommand = new RelayCommand<TeacherDissertationView>((p) => true, (p) => _LoadListTopicCommand(p));
            AddTopic = new RelayCommand<TeacherDissertationView>((p) => true, (p) => _AddTopic(p));

        }
        void _LoadListTopicCommand(TeacherDissertationView topicsView)
        {
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            topicsView.cbxChon.SelectedIndex = 0;
        }
        void _DetailTopicsCommand(TeacherDissertationView topicsView)
        {
            // chuyển sang view detail topic
            TeacherDissertationDetailView detailTopic = new TeacherDissertationDetailView();
            DeTai temp = (DeTai)topicsView.ListTopicView.SelectedItem;
            detailTopic.deTaiId.Text = temp.deTaiId;
            detailTopic.TenDeTai.Text = temp.tenDeTai;
            detailTopic.TenTheLoai.Text = temp.tenTheLoai;
            detailTopic.TenTrangThai.Text = temp.tenTrangThai;
            detailTopic.MoTa.Text = temp.moTa;
            detailTopic.YeuCau.Text = temp.yeuCauChung;
            detailTopic.SoLuong.Text = temp.soLuong.ToString();
            detailTopic.NgayBatDau.Text = temp.ngayBatDau.ToString();
            if (temp.ngayBatDau.ToString() == temp.ngayKetThuc.ToString())
                detailTopic.NgayKetThuc.Text = "";
            else
                detailTopic.NgayKetThuc.Text = temp.ngayKetThuc.ToString();
            string thanhVien = "";
            if (detailTopic.TenTrangThai.Text != "Đã đăng ký")
                thanhVien = "Đề tài chưa đăng ký";
            else
            {
                int nhomId = -1;
                var dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == temp.deTaiId);
                if (dt != null)
                {
                    if (dt.nhomId != null)
                    {
                        nhomId = (int)dt.nhomId;
                    }
                } 
                thanhVien = "Nhóm " + nhomId.ToString();
                //if (nhomId != -1)
                //{
                //    List<SinhVien> sinhViens = svDAO.FindByDeTaiId(nhomId);
                //    if (sinhViens != null)
                //    {
                //        foreach (SinhVien sinhVien in sinhViens)
                //        {
                //            thanhVien += sinhVien.HoTen + "; ";
                //        }
                //    }
                //}
            }
            if (detailTopic.TenTrangThai.Text == "Đề xuất")
                detailTopic.btnXacNhan.Visibility = Visibility.Visible;
            else
                detailTopic.btnXacNhan.Visibility = Visibility.Collapsed;

            ListTopic = Topics;
            topicsView.ListTopicView.ItemsSource = ListTopic;
            topicsView.ListTopicView.SelectedItem = null;
            TeacherMainViewModel.MainFrame.Content = detailTopic;
        }

        void _SearchTopicsCommand(TeacherDissertationView topicsView)
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
                                if (s.tenTrangThai.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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

        void _AddTopic(TeacherDissertationView teacherDissertationView)
        {
            TeacherAddDissertationView addTopicView = new TeacherAddDissertationView();
            TeacherMainViewModel.MainFrame.Content = addTopicView;
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
                    Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, soLuong, tenTrangThai));
            }
            return Topics;
        }
    }
}
