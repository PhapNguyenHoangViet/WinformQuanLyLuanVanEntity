using Group01_QuanLyLuanVan.DAO;
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
        DeTaiDAO dtDAO = new DeTaiDAO();
        SinhVienDAO svDAO = new SinhVienDAO();

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
            detailTopic.deTaiId.Text = temp.DeTaiId;
            detailTopic.TenDeTai.Text = temp.TenDeTai;
            detailTopic.TenTheLoai.Text = temp.TenTheLoai;
            detailTopic.TenTrangThai.Text = temp.TenTrangThai;
            detailTopic.MoTa.Text = temp.MoTa;
            detailTopic.YeuCau.Text = temp.YeuCauChung;
            detailTopic.SoLuong.Text = temp.SoLuong.ToString();
            detailTopic.NgayBatDau.Text = temp.NgayBatDau.ToString();
            if (temp.NgayBatDau.ToString() == temp.NgayKetThuc.ToString())
                detailTopic.NgayKetThuc.Text = "";
            else
                detailTopic.NgayKetThuc.Text = temp.NgayKetThuc.ToString();
            string thanhVien = "";
            if (detailTopic.TenTrangThai.Text != "Đã đăng ký")
                thanhVien = "Đề tài chưa đăng ký";
            else
            {
                int nhomId = svDAO.FindNhomIDByDeTaiId(temp.DeTaiId);
                thanhVien += "Nhóm " + nhomId.ToString();
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
                                if (s.TenTrangThai.ToLower().Contains(topicsView.txbSearch.Text.ToLower()))
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
