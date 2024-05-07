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
    public class StudentScoreViewModel2 : BaseViewModel
    {
        DeTaiDAO dtDAO = new DeTaiDAO();

        public ICommand LoadDiemCommand { get; set; }
        private ObservableCollection<DeTai> _MotDiem;
        public ObservableCollection<DeTai> MotDiem { get => _MotDiem; set { _MotDiem = value;/* OnPropertyChanged();*/ } }
        public StudentScoreViewModel2()
        {
            MotDiem = new ObservableCollection<DeTai>();
            var diemData = dtDAO.LoadDiemByUsername();
            foreach (DataRow row in diemData.Rows)
            {

                float diem = Convert.ToInt32(row["diem"]);

                MotDiem.Add(new DeTai(diem));
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
            var diemData = dtDAO.LoadDiemByUsername();
            foreach (DataRow row in diemData.Rows)
            {

                float diem = Convert.ToInt32(row["diem"]);

                MotDiem.Add(new DeTai(diem));
            }
            return MotDiem;
        }
    }
}
