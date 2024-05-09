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
    public class StudentScoreViewModel2 : BaseViewModel
    {

        public ICommand LoadDiemCommand { get; set; }
        private ObservableCollection<DeTai> _MotDiem;
        public ObservableCollection<DeTai> MotDiem { get => _MotDiem; set { _MotDiem = value;/* OnPropertyChanged();*/ } }
        public StudentScoreViewModel2()
        {

            MotDiem = new ObservableCollection<DeTai>();
            var diemQuery = (from dt in DataProvider.Ins.DB.DeTais
                             join sv in DataProvider.Ins.DB.SinhViens on dt.nhomId equals sv.nhomId
                             where sv.username == Const.sinhVien.username
                             select dt.diem)
                .ToList();
            if (diemQuery.Any())
            {
                int firstDiem = (int)diemQuery.FirstOrDefault();
                MotDiem.Add(new DeTai(firstDiem));

            }
            LoadDiemCommand = new RelayCommand<StudentScoreView2>((p) => true, (p) => _LoadDiemCommand(p));

        }
        void _LoadDiemCommand(StudentScoreView2 diemview)
        {
            diemview.Diemview.ItemsSource = motDiem();
        }
        ObservableCollection<DeTai> motDiem()
        {
            MotDiem = new ObservableCollection<DeTai>();
            var diemQuery = (from dt in DataProvider.Ins.DB.DeTais
                             join sv in DataProvider.Ins.DB.SinhViens on dt.nhomId equals sv.nhomId
                             where sv.username == Const.sinhVien.username
                             select dt.diem)
                .ToList();
            if (diemQuery.Any())
            {
                int firstDiem = (int)diemQuery.FirstOrDefault();
                MotDiem.Add(new DeTai(firstDiem));

            }
            return MotDiem;
        }
    }
}
