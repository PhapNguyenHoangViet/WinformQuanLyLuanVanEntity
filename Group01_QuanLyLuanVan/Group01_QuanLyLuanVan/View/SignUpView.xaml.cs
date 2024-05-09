using Group01_QuanLyLuanVan.Model;
using System.Windows;
namespace Group01_QuanLyLuanVan.View
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : Window
    {
        ComboBoxData comboBoxData = new ComboBoxData();
        public SignUpView()
        {
            InitializeComponent();
            tenKhoa.ItemsSource = comboBoxData.TenKhoa();
        }
    }
}
