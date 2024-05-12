using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentNotiViewModel : BaseViewModel
    {

        private ObservableCollection<ThongBao> _ListThongBao;
        public ObservableCollection<ThongBao> ListThongBao { get => _ListThongBao; set { _ListThongBao = value;/* OnPropertyChanged();*/ } }
        public ICommand DetailThongBaoCommand { get; set; }
        public ObservableCollection<ThongBao> ThongBaos { get; set; }
        public ICommand LoadThongBaosCommand { get; set; }

        private string _selectedThongBaoNoiDung;
        public string SelectedThongBaoNoiDung
        {
            get { return _selectedThongBaoNoiDung; }
            set
            {
                _selectedThongBaoNoiDung = value;
                OnPropertyChanged(nameof(SelectedThongBaoNoiDung));
            }
        }

        public StudentNotiViewModel()
        {
            DeTai dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.nhomId == Const.sinhVien.nhomId);
            string dtTaiId = dt.deTaiId;
            var thongBaos = DataProvider.Ins.DB.ThongBaos.Where(tb => tb.deTaiId == dtTaiId).ToList();
            foreach (ThongBao tb in thongBaos)
            {
                int thongBaoId = tb.thongBaoId;
                int trangThai = Convert.ToInt32(tb.trangthai);
                string tieuDe = tb.tieude;
                string noiDung = tb.noiDung;
                string deTaiId = tb.deTaiId;
                DateTime ngay = Convert.ToDateTime(tb.ngay);
                string tenTrangThai = "";

                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đọc";
                }
                else
                {
                    tenTrangThai = "Chưa đọc";
                }
                ThongBao tbao = new ThongBao();
                tbao.deTaiId = deTaiId;
                tbao.thongBaoId = thongBaoId;
                tbao.tieude = tieuDe;
                tbao.ngay = ngay;
                tbao.tenTrangThai = tenTrangThai;
                ThongBaos.Add(tbao);
            }
            ListThongBao = ThongBaos;
            LoadThongBaosCommand = new RelayCommand<StudentNotiView>((p) => true, (p) => _LoadThongBaosCommand(p));
            DetailThongBaoCommand = new RelayCommand<StudentNotiView>((p) => { return p.ListThongBaoView.SelectedItem == null ? false : true; }, (p) => _DetailThongBaoCommand(p));
        }
        void _LoadThongBaosCommand(StudentNotiView notiView)
        {
            notiView.ListThongBaoView.ItemsSource = listThongBao();
        }
        ObservableCollection<ThongBao> listThongBao()
        {
            ThongBaos = new ObservableCollection<ThongBao>();
            var thongBaos = DataProvider.Ins.DB.ThongBaos
            .Where(tb => tb.DeTai.Nhom.SinhViens.Any(sv => sv.sinhVienId == Const.sinhVien.sinhVienId)).ToList();
            foreach (ThongBao tb in thongBaos)
            {
                int thongBaoId = tb.thongBaoId;
                int trangThai = Convert.ToInt32(tb.trangthai);
                string tieuDe = tb.tieude;
                string noiDung = tb.noiDung;
                string deTaiId = tb.deTaiId;
                DateTime ngay = Convert.ToDateTime(tb.ngay);
                string tenTrangThai = "";

                if (trangThai == 1)
                {
                    tenTrangThai = "Đã đọc";
                }
                else
                {
                    tenTrangThai = "Chưa đọc";
                }

                ThongBao tbao = new ThongBao();
                tbao.deTaiId = deTaiId;
                tbao.thongBaoId = thongBaoId;
                tbao.tieude = tieuDe;
                tbao.ngay = ngay;
                tbao.tenTrangThai = tenTrangThai;
                ThongBaos.Add(tbao);
            }
            return ThongBaos;
        }
        void _DetailThongBaoCommand(StudentNotiView tbView)
        {
            if (tbView != null && tbView.ListThongBaoView.SelectedItem != null)
            {
                var selectedThongBao = (ThongBao)tbView.ListThongBaoView.SelectedItem;
                SelectedThongBaoNoiDung = selectedThongBao.noiDung;

            }
            var thongBaos = DataProvider.Ins.DB.ThongBaos
                .Where(tb => tb.DeTai.nhomId == Const.sinhVien.nhomId && tb.noiDung == SelectedThongBaoNoiDung)
                .ToList();

            foreach (var thongBao in thongBaos)
            {
                thongBao.trangthai = 1;
            }
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
