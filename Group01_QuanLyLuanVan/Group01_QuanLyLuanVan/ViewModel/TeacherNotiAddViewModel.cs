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
    {   public ObservableCollection<DeTai> Topics { get; set; }
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

            DataProvider.Ins.DB.ThongBaos.Add(tb);
            DataProvider.Ins.DB.SaveChanges();
            TeacherNotiDetailView teacherNotiDetailView = new TeacherNotiDetailView();
            teacherNotiDetailView.ListThongBaoView.ItemsSource = listTopic();
            teacherNotiDetailView.ListThongBaoView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = teacherNotiDetailView;
        }

        ObservableCollection<ThongBao> listTopic()
        {
            ThongBaos = new ObservableCollection<ThongBao>();
            
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
