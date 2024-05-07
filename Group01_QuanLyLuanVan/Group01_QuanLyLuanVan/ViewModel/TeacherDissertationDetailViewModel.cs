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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherDissertationDetailViewModel : BaseViewModel
    {
        public ObservableCollection<DeTai> Topics { get; set; }
        DeTaiDAO deTaiDAO = new DeTaiDAO();
        public ICommand back { get; set; }

        public ICommand UpdateTopicCM { get; set; }
        public ICommand UpdateTrangThaiTopicCM { get; set; }

        
        public ICommand DeleteTopicCM { get; set; }
        public TeacherDissertationDetailViewModel()
        {
            back = new RelayCommand<TeacherDissertationDetailView>((p) => true, p => _back(p));
            UpdateTopicCM = new RelayCommand<TeacherDissertationDetailView>((p) => true, (p) => _UpdateTopicCM(p));
            UpdateTrangThaiTopicCM = new RelayCommand<TeacherDissertationDetailView>((p) => true, (p) => _UpdateTrangThaiTopicCM(p));
            DeleteTopicCM = new RelayCommand<TeacherDissertationDetailView>((p) => true, (p) => _DeleteTopicCM(p));
        }

        void _back(TeacherDissertationDetailView paramater)
        {
            TeacherDissertationView topicsView = new TeacherDissertationView();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }
        void _UpdateTopicCM(TeacherDissertationDetailView p)
        {
            string deTaiId = p.deTaiId.Text;
            string tenDeTai = p.TenDeTai.Text;
            string moTa = p.MoTa.Text;
            string yeuCau = p.YeuCau.Text;
            int soLuong = int.Parse(p.SoLuong.Text);
            DeTai dt = new DeTai(deTaiId, tenDeTai, moTa, yeuCau, soLuong);
            deTaiDAO.UpdateTopic(dt);
            MessageBox.Show("Đã cập nhật đề tài này !", "THÔNG BÁO", MessageBoxButton.OK);
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }

        void _UpdateTrangThaiTopicCM(TeacherDissertationDetailView p)
        {
            string deTaiId = p.deTaiId.Text;
            deTaiDAO.UpdateTrangThaiTopic(deTaiId);
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }

        void _DeleteTopicCM(TeacherDissertationDetailView p)
        {
            deTaiDAO.AnTopic(p.deTaiId.Text);
            MessageBox.Show("Đã xóa đề tài này !", "THÔNG BÁO", MessageBoxButton.OK);
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }

        ObservableCollection<DeTai> listTopic()
        {
            Topics = new ObservableCollection<DeTai>();
            var topicsData = deTaiDAO.LoadListTopic(Const.giangVien.GiangVienId);
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
