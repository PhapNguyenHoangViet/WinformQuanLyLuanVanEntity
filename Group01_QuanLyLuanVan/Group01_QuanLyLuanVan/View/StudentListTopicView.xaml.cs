using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for StudentListTopicView.xaml
    /// </summary>
    public partial class StudentListTopicView : Page
    {
        public StudentListTopicView()
        {
            InitializeComponent();
        }
        
    }
    public class TenTrangThaiToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string tenTrangThai)
            {
                if (tenTrangThai == "Đã đăng ký")
                    return new SolidColorBrush(Colors.Green);
                else if (tenTrangThai == "Chưa đăng ký")
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Transparent); // Mặc định trả về màu trong suốt
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
