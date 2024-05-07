using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Group01_QuanLyLuanVan.View
{
    /// <summary>
    /// Interaction logic for TeacherStatisticView.xaml
    /// </summary>
    public partial class TeacherStatisticView : Page
    {
        public TeacherStatisticView()
        {
            InitializeComponent();
        }

        public void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;
            foreach (PieSeries pieSeries in chart.Series)
                pieSeries.PushOut = 0;
            var selecionarSeries = (PieSeries)chartPoint.SeriesView;
            selecionarSeries.PushOut = 0;
        }
    }
}
