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
    public class StudentThesisViewModel : BaseViewModel
    {
        DeTaiDAO dtDAO = new DeTaiDAO();
        public ICommand LoadDeTaiCommand { get; set; }
        public ObservableCollection<DeTai> MyDeTai { get; set; }
        private ObservableCollection<DeTai> _ListTopic;

        public ObservableCollection<DeTai> ListTopic { get => _ListTopic; set { _ListTopic = value;OnPropertyChanged(); } }

        public StudentThesisViewModel()
        {
            LoadDeTaiCommand = new RelayCommand<StudentThesisView>((p) => true, (p) => _LoadDeTaiCommand(p));

            MyDeTai = new ObservableCollection<DeTai>();
            var detaiData = dtDAO.LoadDeTaiByUsername();
            foreach (DataRow row in detaiData.Rows)
            {
                string tenDeTai = row["tenDeTai"].ToString();
                string tenTheLoai = row["tenTheLoai"].ToString();
                string hoTen = row["hoTen"].ToString();
                string moTa = row["moTa"].ToString();
                string yeuCauChung = row["yeuCauChung"].ToString();
                DateTime ngayBatDau = Convert.ToDateTime(row["ngayBatDau"]);
                DateTime ngayKetThuc = Convert.ToDateTime(row["ngayKetThuc"]);
                int nhomId = Convert.ToInt32(row["nhomId"]);
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
            ListTopic = new ObservableCollection<DeTai>();
            var detaiData = dtDAO.LoadDeTaiByUsername();
            foreach (DataRow row in detaiData.Rows)
            {
                string tenDeTai = row["tenDeTai"].ToString();
                string tenTheLoai = row["tenTheLoai"].ToString();
                string hoTen = row["hoTen"].ToString();
                string moTa = row["moTa"].ToString();
                string yeuCauChung = row["yeuCauChung"].ToString();
                DateTime ngayBatDau = Convert.ToDateTime(row["ngayBatDau"]);
                DateTime ngayKetThuc = Convert.ToDateTime(row["ngayKetThuc"]);
                int nhomId = Convert.ToInt32(row["nhomId"]);
                ListTopic.Add(new DeTai(tenDeTai, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, nhomId, hoTen, tenTheLoai));
            }
            return ListTopic;
        }

    }
}
