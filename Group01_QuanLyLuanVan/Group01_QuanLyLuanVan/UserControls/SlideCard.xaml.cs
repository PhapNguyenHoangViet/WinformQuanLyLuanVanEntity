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

namespace Group01_QuanLyLuanVan.UserControls
{
    /// <summary>
    /// Interaction logic for SlideCard.xaml
    /// </summary>
    public partial class SlideCard : UserControl
    {

        private int currentProfile = 0;
        public SlideCard()
        {
            InitializeComponent();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int index = (int)slider.Value;

            switch (index)
            {
                case 0:
                    grid0.Visibility = Visibility.Visible;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Collapsed;
                    currentProfile = 0;
                    break;
                case 1:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Visible;
                    grid2.Visibility = Visibility.Collapsed;
                    currentProfile = 1;
                    break;
                case 2:
                    grid0.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Collapsed;
                    grid2.Visibility = Visibility.Visible;
                    currentProfile = 2;
                    break;
                default:
                    break;
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentProfile--;
            if (currentProfile < 0)
            {
                currentProfile = 2;
            }
            slider.Value = currentProfile;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentProfile++;
            if (currentProfile > 2)
            {
                currentProfile = 0;
            }
            slider.Value = currentProfile;
        }
    }
}
