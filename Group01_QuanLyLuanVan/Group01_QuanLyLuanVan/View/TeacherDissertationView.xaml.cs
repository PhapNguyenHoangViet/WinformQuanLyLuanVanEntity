using Group01_QuanLyLuanVan.ViewModel;
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
    /// Interaction logic for TeacherDissertationView.xaml
    /// </summary>
    public partial class TeacherDissertationView : Page
    {
        public TeacherDissertationView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TeacherAddDissertationView addTopicView = new TeacherAddDissertationView();
            TeacherMainViewModel.MainFrame.Content = addTopicView;
        }
    }
    public class TenTrangThaiToColorConverterTeacher : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string tenTrangThai)
            {
                if (tenTrangThai == "Đã đăng ký")
                    return new SolidColorBrush(Colors.Green);
                else if (tenTrangThai == "Chưa đăng ký")
                    return new SolidColorBrush(Colors.Red);
                else if (tenTrangThai == "Đề xuất") // Thêm điều kiện cho trạng thái "Đề xuất"
                    return new SolidColorBrush(Colors.Yellow); // Trả về màu vàng
            }
            return new SolidColorBrush(Colors.Transparent); // Mặc định trả về màu trong suốt
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
