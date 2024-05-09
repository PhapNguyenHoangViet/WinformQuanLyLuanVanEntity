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
    public class StudentThesisViewModel : BaseViewModel
    {
        public ICommand LoadDeTaiCommand { get; set; }
        public ObservableCollection<DeTai> MyDeTai { get; set; }
        private ObservableCollection<DeTai> _ListTopic;

        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value; OnPropertyChanged(); } }

        public StudentThesisViewModel()
        {
            LoadDeTaiCommand = new RelayCommand<StudentThesisView>((p) => true, (p) => _LoadDeTaiCommand(p));

            MyDeTai = new ObservableCollection<DeTai>();
            var deTaiQuery = (from dt in DataProvider.Ins.DB.DeTais
                              join sv in DataProvider.Ins.DB.SinhViens on dt.nhomId equals sv.nhomId
                              join gv in DataProvider.Ins.DB.GiangViens on dt.giangVienId equals gv.giangVienId
                              join tl in DataProvider.Ins.DB.TheLoais on dt.theLoaiId equals tl.theLoaiId
                              where sv.username == Const.sinhVien.username
                              select dt).ToList();

            foreach (DeTai dt in deTaiQuery)
            {
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;
                string hoTen = dt.hoTen;
                string moTa = dt.moTa;
                string yeuCauChung = dt.yeuCauChung;
                DateTime ngayBatDau = Convert.ToDateTime(dt.ngayBatDau);
                DateTime ngayKetThuc = Convert.ToDateTime(dt.ngayKetThuc);
                int nhomId = Convert.ToInt32(dt.nhomId);
                MyDeTai.Add(new DeTai(tenDeTai, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, nhomId, hoTen, tenTheLoai));
            }
            ListTopic = MyDeTai;
        }
        void _LoadDeTaiCommand(StudentThesisView detaiView)
        {
            detaiView.ListTopicView.ItemsSource = OneDeTai();

        }
        ObservableCollection<DeTai> OneDeTai()
        {
            MyDeTai = new ObservableCollection<DeTai>();
            var deTaiQuery = (from dt in DataProvider.Ins.DB.DeTais
                              join sv in DataProvider.Ins.DB.SinhViens on dt.nhomId equals sv.nhomId
                              join gv in DataProvider.Ins.DB.GiangViens on dt.giangVienId equals gv.giangVienId
                              join tl in DataProvider.Ins.DB.TheLoais on dt.theLoaiId equals tl.theLoaiId
                              where sv.username == Const.sinhVien.username
                              select dt).ToList();

            foreach (DeTai dt in deTaiQuery)
            {
                string tenDeTai = dt.tenDeTai;
                string tenTheLoai = dt.TheLoai.tenTheLoai;

                string hoTen = dt.hoTen;
                string moTa = dt.moTa;
                string yeuCauChung = dt.yeuCauChung;
                DateTime ngayBatDau = Convert.ToDateTime(dt.ngayBatDau);
                DateTime ngayKetThuc = Convert.ToDateTime(dt.ngayKetThuc);
                int nhomId = Convert.ToInt32(dt.nhomId);
                MyDeTai.Add(new DeTai(tenDeTai, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, nhomId, hoTen, tenTheLoai));
            }
            return MyDeTai;
        }

    }
}
