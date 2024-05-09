using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for StudentAddTopicsView.xaml
    /// </summary>
    public partial class StudentAddTopicsView : Page
    {
        private ObservableCollection<SinhVien> sinhViens;

        public StudentAddTopicsView()
        {
            InitializeComponent();
            LoadSinhVienData();
            LoadGiangVienData();
            LoadTheLoaiData();
        }

        private void LoadTheLoaiData()
        {
            var dataTable = DataProvider.Ins.DB.TheLoais.Where(dt => dt.khoaId == Const.sinhVien.khoaId).ToList();

            foreach (TheLoai tl in dataTable)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = tl.tenTheLoai.ToString();
                TheLoai.Items.Add(item);
            }
        }
        private void LoadGiangVienData()
        {

            var dataTable = DataProvider.Ins.DB.GiangViens.Where(dt => dt.khoaId == Const.sinhVien.khoaId).ToList();

            foreach (GiangVien tl in dataTable)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = tl.hoTen.ToString();
                TheLoai.Items.Add(item);
            }

            
        }
        private void LoadSinhVienData()
        {

            var dataTable = DataProvider.Ins.DB.SinhViens.Where(dt => dt.khoaId == Const.sinhVien.khoaId && dt.username != Const.taiKhoan.username).ToList();
            sinhViens = new ObservableCollection<SinhVien>();

            foreach (SinhVien tl in dataTable)
            {
                sinhViens.Add(new SinhVien
                {
                    sinhVienId = tl.sinhVienId.ToString(),
                    hoTen = tl.hoTen.ToString(),
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
            if (selectedCount > 2)
            {
                MessageBox.Show("Số lượng thành viên không được vượt quá 3", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    selectedItemTextBlock.Text += sinhVien.hoTen + ", ";
                }
            }
            selectedItemTextBlock.Text = selectedItemTextBlock.Text.TrimEnd(' ', ',');
        }
    }
}
