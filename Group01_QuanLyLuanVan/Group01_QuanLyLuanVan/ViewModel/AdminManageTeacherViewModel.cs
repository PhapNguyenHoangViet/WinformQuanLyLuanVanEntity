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
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class AdminManageTeacherViewModel:BaseViewModel
    {
        GiangVienDAO gvDAO = new GiangVienDAO();
        private ObservableCollection<GiangVien> listGV;
        public ObservableCollection<GiangVien> ListGV { get => listGV; set { listGV = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTK;
        public ObservableCollection<string> ListTK { get => _ListTK; set { _ListTK = value; OnPropertyChanged(); } }
        public ICommand SearchGVCommand { get; set; }
        public ICommand DetailGVCommand { get; set; }
        public ICommand LoadListGVCommand { get; set; }
        public ObservableCollection<GiangVien> GVs { get; set; }

        public AdminManageTeacherViewModel()
        {
            GVs = new ObservableCollection<GiangVien>();
            var gvsData = gvDAO.ListGiangVienTot();
            foreach (DataRow row in gvsData.Rows)
            {

                string giangVienId = row["giangVienId"].ToString();
                string hoTen = row["TenGiangVien"].ToString();
                string SoLuongTaskGiao = row["SoLuongTaskGiao"].ToString();

                GVs.Add(new GiangVien( giangVienId,  hoTen, SoLuongTaskGiao));
            }
            ListGV = GVs;
            ListTK = new ObservableCollection<string>() { "Mã số", "Giảng viên" };
            DetailGVCommand = new RelayCommand<AdminManageTeacherView>((p) => { return p.ListViewGV.SelectedItem == null ? false : true; }, (p) => _DetailGVCommand(p));
            SearchGVCommand = new RelayCommand<AdminManageTeacherView>((p) => { return p == null ? false : true; }, (p) => _SearchTeachersCommand(p));
            LoadListGVCommand = new RelayCommand<AdminManageTeacherView>((p) => true, (p) => _LoadListGVCommand(p));
          

        }
        void _LoadListGVCommand(AdminManageTeacherView GVsView)
        {
            GVsView.ListViewGV.ItemsSource = _ListTeacher();
            GVsView.ListViewGV.Items.Refresh();
            GVsView.cbxChon.SelectedIndex = 0;
        }
        void _DetailGVCommand(AdminManageTeacherView GVsView)
        {

        }

        void _SearchTeachersCommand(AdminManageTeacherView teachersView)
        {
            ObservableCollection<GiangVien> temp = new ObservableCollection<GiangVien>();
            if (teachersView.cbxChon.Text != "")
            {
                switch (teachersView.cbxChon.SelectedItem.ToString())
                {
                    case "Mã số":
                        {
                            foreach (GiangVien s in ListGV)
                            {
                                if (s.GiangVienId.ToLower().Contains(teachersView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                    default:
                        {
                            foreach (GiangVien s in ListGV)
                            {
                                if (s.HoTen.ToLower().Contains(teachersView.txbSearch.Text.ToLower()))
                                {
                                    temp.Add(s);
                                }
                            }
                            break;
                        }
                }
                teachersView.ListViewGV.ItemsSource = temp;
            }
            else
                teachersView.ListViewGV.ItemsSource = ListGV;
        }

        ObservableCollection<GiangVien> _ListTeacher()
        {

            GVs = new ObservableCollection<GiangVien>();
            var gvsData = gvDAO.ListGiangVienTot();
            foreach (DataRow row in gvsData.Rows)
            {
                string giangVienId = row["giangVienId"].ToString();
                string hoTen = row["TenGiangVien"].ToString();
                string SoLuongTaskGiao = row["SoLuongTaskGiao"].ToString();

                GVs.Add(new GiangVien(giangVienId, hoTen, SoLuongTaskGiao));
            }
            return GVs;
        }
    }
}
