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
using System.Windows;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherAddDissertationViewModel : BaseViewModel
    {
        GiangVienDAO gvDAO = new GiangVienDAO(); 
        TheLoaiDAO tlDAO = new TheLoaiDAO();
        DeTaiDAO dtDAO = new DeTaiDAO();
        public ObservableCollection<DeTai> Topics { get; set; }
        public ICommand back { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand chooseTL { get; set; }
        public ICommand AddDissertation { get; set; }

        private List<TheLoai> _LTL;
        public List<TheLoai> LTL { get => _LTL; set { _LTL = value; OnPropertyChanged(); } }

        public TeacherAddDissertationViewModel()
        {
            back = new RelayCommand<TeacherAddDissertationView>((p) => true, p => _back(p));
            Loadwd = new RelayCommand<TeacherAddDissertationView>((p) => true, (p) => _Loadwd(p));
            chooseTL = new RelayCommand<TeacherAddDissertationView>((p) => true, (p) => _chooseTL(p));
            AddDissertation = new RelayCommand<TeacherAddDissertationView>((p) => true, (p) => _AddDissertation(p));
        }

        void _AddDissertation(TeacherAddDissertationView paramater)
        {
            if (paramater.TenDeTai.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (paramater.SoLuong.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(paramater.SoLuong.Text, @"^\d+$"))
            {
                System.Windows.MessageBox.Show("Số lượng chỉ được nhập số !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (paramater.MoTa.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (paramater.YeuCau.Text == "")
            {
                System.Windows.MessageBox.Show("Bạn cần nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (paramater.LTL.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Bạn chưa chọn thể loại !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TheLoai tl = (TheLoai)paramater.LTL.SelectedItem;
            DeTai dl = new DeTai(paramater.TenDeTai.Text, paramater.MoTa.Text, paramater.YeuCau.Text, int.Parse(paramater.SoLuong.Text), 0, DateTime.Parse(paramater.NgayBatDau.Text), DateTime.Parse(paramater.NgayBatDau.Text), tl.TheLoaiId, Const.giangVien.GiangVienId);
            dtDAO.AddTopic(dl);
            TeacherDissertationView topicsView = new TeacherDissertationView();
            topicsView.ListTopicView.ItemsSource = listTopic();
            topicsView.ListTopicView.Items.Refresh();
            TeacherMainViewModel.MainFrame.Content = topicsView;
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

        void _Loadwd(TeacherAddDissertationView paramater)
        {
            GiangVien gv = Const.giangVien;
            LTL = tlDAO.FindByKhoaId(gv.KhoaId);
            paramater.LTL.ItemsSource = LTL;
            paramater.NgayBatDau.SelectedDate = DateTime.Today;
        }
        void _chooseTL(TeacherAddDissertationView parameter)
        {
            TheLoai temp = (TheLoai)parameter.LTL.SelectedItem;
        }
        void _back(TeacherAddDissertationView paramater)
        {
            TeacherDissertationView topicsView = new TeacherDissertationView();
            TeacherMainViewModel.MainFrame.Content = topicsView;
        }
    }
}
