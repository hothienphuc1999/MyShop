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
using MyShop.Model;

namespace MyShop.UC.MasterData
{
    /// <summary>
    /// Interaction logic for EditCategoryWindow.xaml
    /// </summary>
    public partial class EditCategoryWindow : Window
    {
        public EditCategoryWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var db = new MyShopEntities();
            if (catNameTextBox.Text != "")
            {
                var category = new Category()
                {
                    Name = catNameTextBox.Text
                };
                db.Categories.Add(category);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                MessageBox.Show($"Category \"{catNameTextBox.Text}\" is added!","Successfully",MessageBoxButton.OK,MessageBoxImage.Information);
                Close();
            }
            else
                MessageBox.Show("Category name is emty","Warning", MessageBoxButton.OK,MessageBoxImage.Warning);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CatNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (catNameTextBox.Text.Length == 0)
            {
                catNameLabel.Visibility = Visibility.Visible;
            }
        }

        private void CatNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            catNameLabel.Visibility = Visibility.Hidden;
        }
    }
}
