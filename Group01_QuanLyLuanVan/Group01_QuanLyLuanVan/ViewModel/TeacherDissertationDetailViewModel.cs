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
            var dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == deTaiId);
            if (dt != null)
            {
                dt.tenDeTai = tenDeTai;
                dt.moTa = moTa;
                dt.yeuCauChung = yeuCau;
                dt.soLuong = soLuong;
                DataProvider.Ins.DB.DeTais.Attach(dt);
                DataProvider.Ins.DB.SaveChanges();
            }

            MessageBox.Show("Đã cập nhật đề tài này !", "THÔNG BÁO", MessageBoxButton.OK);
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }

        void _UpdateTrangThaiTopicCM(TeacherDissertationDetailView p)
        {
            string deTaiId = p.deTaiId.Text;
            DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == deTaiId);
            if (dt != null)
            {
                dt.trangThai = 1;
                DataProvider.Ins.DB.SaveChanges();
            }
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }

        void _DeleteTopicCM(TeacherDissertationDetailView p)
        {
            var dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == p.deTaiId.Text);
            if (dt != null)
            {
                dt.an = 1;
                DataProvider.Ins.DB.SaveChanges();
            }
            MessageBox.Show("Đã xóa đề tài này !", "THÔNG BÁO", MessageBoxButton.OK);
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
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
