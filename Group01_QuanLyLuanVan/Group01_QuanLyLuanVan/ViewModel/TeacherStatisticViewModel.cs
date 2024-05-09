using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
using LiveCharts.Defaults;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherStatisticViewModel:BaseViewModel
    {
        public ICommand LoadCommand { get; set; }

        private string _gioi;
        public string Gioi { get => _gioi; set { _gioi = value; OnPropertyChanged(); } }
        private string _kha;
        public string Kha { get => _kha; set { _kha = value; OnPropertyChanged(); } }
        private string _trungBinh;
        public string TrungBinh { get => _trungBinh; set { _trungBinh = value; OnPropertyChanged(); } }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }

        private SeriesCollection _KetQuaData;
        public SeriesCollection KetQuaData
        {
            get { return _KetQuaData; }
            set
            {
                _KetQuaData = value;
                OnPropertyChanged(nameof(KetQuaData));
            }
        }
        public TeacherStatisticViewModel()
        {
            var countGioi = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.diem <= 10 && dt.diem >= 8 && dt.an != 1)
                .Count();
            var countKha = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.diem < 8 && dt.diem >= 6.5 && dt.an != 1)
                .Count();
            var countTrungBinh = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.diem < 6.5 && dt.diem >= 5 && dt.an != 1)
                .Count();
            Gioi = countGioi.ToString();
            Kha = countKha.ToString();
            TrungBinh = countTrungBinh.ToString();

            List<int> ListSoYcCHT = new List<int>();
            List<int> ListSoYcHT = new List<int>();
            List<string> ListYcId = new List<string>();
            var yeuCauData = DataProvider.Ins.DB.YeuCaus
                        .GroupBy(yc => yc.deTaiId)
                        .Select(group => new {
                            DeTaiId = group.Key,
                            SoYeuCau = group.Count(),
                            SoYeuCauHoanThanh = group.Count(yc => yc.trangThai == 100)
                        })
                        .ToList();

            foreach (var yc in yeuCauData)
            {
                ListSoYcHT.Add(yc.SoYeuCauHoanThanh);
                ListSoYcCHT.Add(yc.SoYeuCau - yc.SoYeuCauHoanThanh);
                ListYcId.Add(yc.DeTaiId);
            }

            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Title = "Số yêu cầu hoàn thành",
                    Values = new ChartValues<int>(ListSoYcHT),
                },
                new StackedColumnSeries
                {
                    Title = "Số yêu cầu chưa hoàn thành",
                    Values = new ChartValues<int>(ListSoYcCHT),
                    Fill = new SolidColorBrush(Colors.Orange)
                }
            };
            Labels = ListYcId;

            int gioiCount = int.Parse(Gioi);
            int khaCount = int.Parse(Kha);
            int trungBinhCount = int.Parse(TrungBinh);

            KetQuaData = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Giỏi",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(gioiCount) },
                    DataLabels = true,
                    LabelPoint = point => string.Format("{0} ({1:P})", point.Y, point.Participation),
                },
                new PieSeries
                {
                    Title = "Khá",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(khaCount) },
                    DataLabels = true,
                    LabelPoint = point => string.Format("{0} ({1:P})", point.Y, point.Participation),
                },
                new PieSeries
                {
                    Title = "Trung bình",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(trungBinhCount) },
                    DataLabels = true,
                    LabelPoint = point => string.Format("{0} ({1:P})", point.Y, point.Participation),
                }
            };
            LoadCommand = new RelayCommand<TeacherStatisticView>((p) => true, (p) => _LoadCommand(p));

        }
        void _LoadCommand(TeacherStatisticView topicsView)
        {
            var countGioi = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.diem <= 10 && dt.diem >= 8 && dt.an != 1)
                .Count();
            var countKha = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.diem < 8 && dt.diem >= 6.5 && dt.an != 1)
                .Count();
            var countTrungBinh = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.giangVienId == Const.giangVien.giangVienId && dt.diem < 6.5 && dt.diem >= 5 && dt.an != 1)
                .Count();
            Gioi = countGioi.ToString();
            Kha = countKha.ToString();
            TrungBinh = countTrungBinh.ToString();

            List<int> ListSoYcCHT = new List<int>();
            List<int> ListSoYcHT = new List<int>();
            List<string> ListYcId = new List<string>();
            var yeuCauData = DataProvider.Ins.DB.YeuCaus
                        .GroupBy(yc => yc.deTaiId)
                        .Select(group => new {
                            DeTaiId = group.Key,
                            SoYeuCau = group.Count(),
                            SoYeuCauHoanThanh = group.Count(yc => yc.trangThai == 100)
                        })
                        .ToList();

            foreach (var yc in yeuCauData)
            {
                ListSoYcHT.Add(yc.SoYeuCauHoanThanh);
                ListSoYcCHT.Add(yc.SoYeuCau - yc.SoYeuCauHoanThanh);
                ListYcId.Add(yc.DeTaiId);
            }

            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Title = "Số yêu cầu hoàn thành",
                    Values = new ChartValues<int>(ListSoYcHT),
                },
                new StackedColumnSeries
                {
                    Title = "Số yêu cầu chưa hoàn thành",
                    Values = new ChartValues<int>(ListSoYcCHT),
                    Fill = new SolidColorBrush(Colors.Orange)
                }
            };
            Labels = ListYcId;

            int gioiCount = int.Parse(Gioi);
            int khaCount = int.Parse(Kha);
            int trungBinhCount = int.Parse(TrungBinh);

            KetQuaData = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Giỏi",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(gioiCount) },
                    DataLabels = true,
                    LabelPoint = point => string.Format("{0} ({1:P})", point.Y, point.Participation),
                },
                new PieSeries
                {
                    Title = "Khá",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(khaCount) },
                    DataLabels = true,
                    LabelPoint = point => string.Format("{0} ({1:P})", point.Y, point.Participation),
                },
                new PieSeries
                {
                    Title = "Trung bình",
                    Values = new ChartValues<ObservableValue> {new ObservableValue(trungBinhCount) },
                    DataLabels = true,
                    LabelPoint = point => string.Format("{0} ({1:P})", point.Y, point.Participation),
                }
            };
        }




    }
}
