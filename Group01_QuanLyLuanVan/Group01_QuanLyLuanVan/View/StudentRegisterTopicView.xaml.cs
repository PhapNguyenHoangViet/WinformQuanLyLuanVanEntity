using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for StudentRegisterTopicView.xaml
    /// </summary>
    public partial class StudentRegisterTopicView : Page
    {
        private ObservableCollection<SinhVien> sinhViens;
        SinhVienDAO svDAO = new SinhVienDAO();



        public StudentRegisterTopicView()
        {
            InitializeComponent();
            LoadSinhVienData();
        }

        private void LoadSinhVienData()
        {
            DataTable dataTable = svDAO.LoadListSinhVienDangKyDeTai(); // Đây là phần bạn phải cung cấp từ dữ liệu của bạn.
            sinhViens = new ObservableCollection<SinhVien>();

            foreach (DataRow row in dataTable.Rows)
            {
                sinhViens.Add(new SinhVien
                {
                    SinhVienId = row["sinhVienId"].ToString(),
                    HoTen = row["HoTen"].ToString(),
                    IsSelected = false
                });
            }

            multiSelectComboBox.ItemsSource = sinhViens;
        }

        private void multiSelectComboBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                multiSelectComboBox.IsDropDownOpen = true;
            }
        }

        private int selectedCount = 1;
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedCount >= int.Parse(SoLuong.Text))
            {
                MessageBox.Show("Số lượng thành viên vượt quá quy định", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ((CheckBox)sender).IsChecked = false; // Đặt lại checkbox về trạng thái không được chọn               
            }
            selectedCount++;
            UpdateSelectedItemsText();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedCount--;
            UpdateSelectedItemsText();
        }

        private void UpdateSelectedItemsText()
        {

            selectedItemTextBlock.Text = "";
            foreach (SinhVien sinhVien in multiSelectComboBox.ItemsSource)
            {
                if (sinhVien.IsSelected)
                {
                    selectedItemTextBlock.Text += sinhVien.HoTen + ", ";
                }
            }
            selectedItemTextBlock.Text = selectedItemTextBlock.Text.TrimEnd(' ', ',');
        }

    }
}
