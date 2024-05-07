using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherNotiAddViewModel:BaseViewModel
    {
        GiangVienDAO gvDAO = new GiangVienDAO();
        ThongBaoDAO tbDAO = new ThongBaoDAO();
        DeTaiDAO dtDAO = new DeTaiDAO();
        public ObservableCollection<DeTai> Topics { get; set; }
        public ICommand back { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand AddThongBao { get; set; }
        public ObservableCollection<ThongBao> ThongBaos { get; set; }
        public TeacherNotiAddViewModel()
        {
            back = new RelayCommand<TeacherNotiAddView>((p) => true, p => _back(p));
            Loadwd = new RelayCommand<TeacherNotiAddView>((p) => true, (p) => _Loadwd(p));
            AddThongBao = new RelayCommand<TeacherNotiAddView>((p) => true, (p) => _AddThongBao(p));
        }

        void _AddThongBao(TeacherNotiAddView paramater)
        {
            if (paramater.Ngay.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (paramater.TieuDe.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (paramater.NoiDung.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ThongBao tb = new ThongBao(paramater.TieuDe.Text, paramater.NoiDung.Text, Const.deTaiId, DateTime.Parse(paramater.Ngay.Text));
            tbDAO.AddThongBao(tb);
            TeacherNotiDetailView teacherNotiDetailView = new TeacherNotiDetailView();
            teacherNotiDetailView.ListThongBaoView.ItemsSource = listTopic();
            teacherNotiDetailView.ListThongBaoView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = teacherNotiDetailView;
        }

        ObservableCollection<ThongBao> listTopic()
        {
            ThongBaos = new ObservableCollection<ThongBao>();
            var thongBaosData = tbDAO.LoadListThongBao(Const.deTaiId);

            foreach (DataRow row in thongBaosData.Rows)
            {
                int thongBaoId = Convert.ToInt32(row["thongBaoId"]);
                string tieuDe = row["tieuDe"].ToString();
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                DateTime ngay = Convert.ToDateTime(row["ngay"]);

                ThongBaos.Add(new ThongBao(thongBaoId, tieuDe, noiDung, deTaiId, ngay));
            }
            return ThongBaos;
        }

        void _Loadwd(TeacherNotiAddView paramater)
        {
            paramater.Ngay.Text = DateTime.Today.ToString();
        }
        void _back(TeacherNotiAddView paramater)
        {
            TeacherNotiDetailView teacherNotiDetailView = new TeacherNotiDetailView();
            TeacherMainViewModel.MainFrame.Content = teacherNotiDetailView;
        }
    }
}
