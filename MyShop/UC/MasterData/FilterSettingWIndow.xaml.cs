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
using System.Windows.Shapes;

namespace MyShop.UC.MasterData
{
    public delegate void PassingDataDelegate(int ProductQuantites, int PriceRanges);
    /// <summary>
    /// Interaction logic for FilterSettingWIndow.xaml
    /// </summary>
    public partial class FilterSettingWIndow : Window
    {
        public event PassingDataDelegate Handler;
        public FilterSettingWIndow(int _numberOfPage, int _priceRange)
        {
            InitializeComponent();
            ProductQuantity.ItemsSource = new List<int>() { 1, 2, 3, 4, 5, 6 };
            ProductQuantity.SelectedValue = _numberOfPage;
            if (_priceRange == 0)
            {
                priceCheck.IsChecked = false;
            }
            else
            {
                PriceSlider.Value = _priceRange;
                priceCheck.IsChecked = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (priceCheck.IsChecked == true)
            {
                Handler?.Invoke((int)ProductQuantity.SelectedValue, (int)PriceSlider.Value);
            }
            else
                Handler?.Invoke((int)ProductQuantity.SelectedValue, 0);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
