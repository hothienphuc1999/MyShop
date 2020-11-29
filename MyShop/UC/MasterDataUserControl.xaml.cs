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
using System.Data.Linq.SqlClient;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using MyShop.Model;
using MyShop.UC.MasterData;
using Image = MyShop.Model.Image;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyShop.UC
{
    /// <summary>
    /// Interaction logic for MasterDataUC.xaml
    /// </summary>
    public partial class MasterDataUserControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public enum MasterDataAction
        {
            AddNewCategory,               // Thêm mới một Loại sản phẩm
            DeleteSelectedCategory,   // Xóa Loại sản phẩm đang được chọn
            AddNewProduct,		  // Thêm mới một Sản phẩm
            UpdateSelectedProduct,   // Cập nhật Sản phẩm đang được chọn
            DeleteSelectedProduct     // Xóa Sản phẩm đang được chọn
        };
        public void HandleParentEvent(MasterDataAction action)
        {
            switch (action)
            {
                case MasterDataAction.AddNewCategory:
                    addNewCategory();
                    break;
                case MasterDataAction.DeleteSelectedCategory:
                    DeleteCategory();
                    break;
                case MasterDataAction.AddNewProduct:
                    addNewProduct();
                    break;
                case MasterDataAction.UpdateSelectedProduct:
                    updateSelectedProduct();
                    break;
                case MasterDataAction.DeleteSelectedProduct:
                    deleteSelectedProduct();
                    break;
            }
        }

        private void deleteSelectedProduct()
        {
            if (productsListView.SelectedIndex >= 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Do you want delete product \"{tenSPTextBox.Text}\"?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var db = new MyShopEntities();
                    int id = int.Parse(idSPTextBox.Text);
                    var img = db.Products.Find(id).Images.FirstOrDefault();
                    var deleteItem = (from product in db.Products
                                      where product.ID == id
                                      select product).Single();
                    db.Products.Remove(deleteItem);
                    if (img != null)
                    {
                        db.Images.Remove(img);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    MessageBox.Show($"Product \"{deleteItem.Name}\" is deleted!", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show($"Please select product to delete!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            SearchTextBox.Text = "";

            UpdateData(categoriesComboBox.SelectedItem as Category);
            Paging(categoriesComboBox.SelectedItem as Category);
        }

        private void updateSelectedProduct()
        {
            if (productsListView.SelectedIndex >= 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Product Name: {tenSPTextBox.Text}, SKU: {skuTextBox.Text}, Price: {giaSPTextbox.Text}, Quantity: {slTextbox.Text}, Description: {desTextbox.Text} ",
                    "Do you want update this product?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var db = new MyShopEntities();
                    int id = int.Parse(idSPTextBox.Text);
                    Product product = db.Products.Single(p => p.ID == id);
                    
                    product.Name = tenSPTextBox.Text;
                    product.Price = double.Parse(giaSPTextbox.Text);
                    product.Quantity = int.Parse(slTextbox.Text);
                    product.SKU = skuTextBox.Text;
                    product.Description = desTextbox.Text;


                    if (ProductImageTemp.Source != null)
                    {
                        Image image = db.Products.Find(id).Images.SingleOrDefault();
                        var img = ProductImageTemp.Source as BitmapImage;
                        
                        var encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(img));

                        using (var stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            if (image == null)
                            {
                                Image photo = new Image()
                                {
                                    ProductID = id,
                                    Data = stream.ToArray()
                                };
                                db.Images.Add(photo);
                            }
                            else
                                image.Data = stream.ToArray();
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                    MessageBox.Show($"Product \"{product.Name}\" is changed!", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show($"Please select product to update!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            UpdateData(categoriesComboBox.SelectedItem as Category);
            Paging(categoriesComboBox.SelectedItem as Category);
        }

        private bool CheckNull()
        {
            foreach (UIElement control in ProductInfoPannel.Children)
            {
                if (control is Canvas)
                {
                    var c = control as Canvas;
                    foreach (UIElement uIElement in c.Children)
                    {
                        if (uIElement is TextBox)
                        {
                            TextBox textBox = uIElement as TextBox;
                            if (textBox.Name != "desTextbox" && textBox.Name != "idSPTextBox" && textBox.Text == "")
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        private void addNewProduct()
        {
            if (productsListView.SelectedIndex < 0 && CheckNull() == true)
            {
                var db = new MyShopEntities();
                var selected = categoriesComboBox.SelectedItem as Category;
                var products = new Product()
                {
                    CatID = selected.ID,
                    Name = tenSPTextBox.Text,
                    Price = double.Parse(giaSPTextbox.Text),
                    Quantity = int.Parse(slTextbox.Text),
                    SKU = skuTextBox.Text,
                    Description = desTextbox.Text
                };
                db.Products.Add(products);

                MessageBoxResult dialogResult = MessageBox.Show("Do you want add product image now?", "Add Product Image", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    var screen = new OpenFileDialog();
                    if (screen.ShowDialog() == true)
                    {
                        var filename = screen.FileName;

                        var image = new BitmapImage(new Uri(filename, UriKind.Absolute));
                        var encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));

                        using (var stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            Image photo = new Image()
                            {
                                ProductID = products.ID,
                                Data = stream.ToArray()
                            };

                            db.Images.Add(photo);
                        }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                MessageBox.Show($"New product is added!", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateData(selected);
            }
            else
                MessageBox.Show("Please deselect product and text data in \"Product Information\" area or text enough data");

            SearchTextBox.Text = "";

            UpdateData(categoriesComboBox.SelectedItem as Category);
            Paging(categoriesComboBox.SelectedItem as Category);
        }

        private int _numberOfPage = 2;
        private int _curentPage = 1;
        private int _priceRange = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public MasterDataUserControl()
        {
            InitializeComponent();
            categoriesComboBox.ItemsSource = categories;
            CategoryProductCombobox.ItemsSource = categories;
        }

        private void TenSPTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            tenSPLabel.Visibility = Visibility.Hidden;
        }

        private void TenSPTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tenSPTextBox.Text.Length == 0)
            {
                tenSPLabel.Visibility = Visibility.Visible;
            }
        }

        private void GiaSPTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            giaSPLabel.Visibility = Visibility.Hidden;
        }

        private void GiaSPTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (giaSPTextbox.Text.Length == 0)
            {
                giaSPLabel.Visibility = Visibility.Visible;
            }
        }

        private void SlTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (slTextbox.Text.Length == 0)
            {
                slLabel.Visibility = Visibility.Visible;
            }
        }

        private void SlTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            slLabel.Visibility = Visibility.Hidden;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategory();
            Category selectedCategory = categoriesComboBox.SelectedItem as Category;
            Paging(selectedCategory);
            PageComboBox.SelectedIndex = 0;
            UpdateData(selectedCategory);

        }
        public void LoadCategory()
        {
            MyShopEntities db = new MyShopEntities();
            var oc = new ObservableCollection<Category>(db.Categories);
            foreach (var cat in oc)
            {
                categories.Add(cat);
            }
            categoriesComboBox.SelectedIndex = 0;
        }
        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = categoriesComboBox.SelectedItem as Category;
            Paging(selectedCategory);
            UpdateData(selectedCategory);
            CategoryProductCombobox.SelectedIndex = -1;
        }
        /// <summary>
        /// Load data to ListView
        /// </summary>
        /// <param name="category">Selected Category</param>
        private void UpdateData(Category category)
        {
            if (category!=null)
            {
                MyShopEntities db = new MyShopEntities();
                var products = db.Products;
                var images = db.Images;
                var query = (from product in products
                                 //from image in product.Images
                             join image in images on product.ID equals image.ProductID into im
                             from i in im.DefaultIfEmpty()
                             where (product.CatID == category.ID) && product.Name.Contains(SearchTextBox.Text)
                             && (_priceRange != 0 ? (product.Price > _priceRange - 5000000 && product.Price < _priceRange + 5000000) : product.Price > 0)
                             orderby product.ID
                             select new
                             {
                                 ID = product.ID,
                                 Name = product.Name,
                                 Image = i.Data,
                                 Price = product.Price,
                                 Quantity = product.Quantity,
                                 SKU = product.SKU,
                                 Description = product.Description,
                                 Cat = product.Category.ID
                             }).Skip((_curentPage - 1) * _numberOfPage).Take(_numberOfPage);

                productsListView.ItemsSource = query.ToList();
            }
        }
        private void Paging(Category category)
        {
            if (category != null)
            {
                MyShopEntities db = new MyShopEntities();
                var cat = db.Categories;
                var products = cat.Find(category.ID).Products;
                var productsCount = (from product in products
                                     where product.Name.Contains(SearchTextBox.Text.ToUpper())
                                     && (_priceRange != 0 ? (product.Price > _priceRange - 5000000 && product.Price < _priceRange + 5000000) : product.Price > 0)
                                     orderby product.ID
                                     select product).Count();
                int totalPage = productsCount / _numberOfPage;

                if (productsCount % _numberOfPage != 0)
                {
                    totalPage++;
                }
                _curentPage = 1;
                var pagingInfo = new PagingInfo(totalPage);
                PageComboBox.ItemsSource = pagingInfo.Items.ToList();
                PageComboBox.SelectedIndex = 0;
            }
        }

        private void PageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var category = categoriesComboBox.SelectedItem as Category;

            var next = PageComboBox.SelectedItem as PagingRow;
            if (PageComboBox.SelectedItem !=null)
            {
                _curentPage = next.Page;
            }
            
            UpdateData(category);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = PageComboBox.SelectedIndex;

            if (currentIndex < PageComboBox.Items.Count)
            {
                PageComboBox.SelectedIndex = currentIndex + 1;
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = PageComboBox.SelectedIndex;

            if (currentIndex > 0)
            {
                PageComboBox.SelectedIndex = currentIndex - 1;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedCategory = categoriesComboBox.SelectedItem as Category;
            
            Paging(selectedCategory);
            UpdateData(selectedCategory);
        }

        private void TenSPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tenSPTextBox.Text.Length == 0)
            {
                tenSPLabel.Visibility = Visibility.Visible;
            }
            else
            {
                tenSPLabel.Visibility = Visibility.Hidden;
            }
        }

        private void GiaSPTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (giaSPTextbox.Text.Length == 0)
            {
                giaSPLabel.Visibility = Visibility.Visible;
            }
            else
            {
                giaSPLabel.Visibility = Visibility.Hidden;
            }
        }

        private void SkuTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            skuLabel.Visibility = Visibility.Hidden;
        }

        private void SkuTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (skuTextBox.Text.Length == 0)
            {
                skuLabel.Visibility = Visibility.Visible;
            }
        }

        private void SkuTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (skuTextBox.Text.Length == 0)
                skuLabel.Visibility = Visibility.Visible;
            else
                skuLabel.Visibility = Visibility.Hidden;
        }

        private void SlTextbox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            if (slTextbox.Text.Length == 0)
            {
                slLabel.Visibility = Visibility.Visible;
            }
        }

        private void SlTextbox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            slLabel.Visibility = Visibility.Hidden;
        }

        private void SlTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (slTextbox.Text.Length == 0)
            {
                slLabel.Visibility = Visibility.Visible;
            }
            else
                slLabel.Visibility = Visibility.Hidden;
        }

        private void DesTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            desLabel.Visibility = Visibility.Hidden;
        }

        private void DesTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (desTextbox.Text.Length == 0)
            {
                desLabel.Visibility = Visibility.Visible;
            }
        }

        private void DesTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (desTextbox.Text.Length == 0)
            {
                desLabel.Visibility = Visibility.Visible;
            }
            else
                desLabel.Visibility = Visibility.Hidden;
        }

        private void ProductsListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
            {
                productsListView.UnselectAll();
                CategoryProductCombobox.SelectedIndex = -1;
            }
        }
        private void addNewCategory()
        {
            var screen = new EditCategoryWindow();
            screen.ShowDialog();
            var db = new MyShopEntities();
            int countCat = db.Categories.Count();
            if (categories.Count() != countCat)
            {
                var newCat = db.Categories.ToList();
                var lastItem = newCat.Last();
                categoriesComboBox.SelectedIndex = 0;
                categories.Add(lastItem);
            }
        }
        private void DeleteCategory()
        {
            var db = new MyShopEntities();
            var selected = categoriesComboBox.SelectedItem as Category;
            var deleteItem = (from x in db.Categories
                              where x.ID == selected.ID
                              select x).First();
            db.Categories.Remove(deleteItem);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show($"Category \"{selected.Name}\" is deleted!", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            categoriesComboBox.SelectedIndex = 0;
            categories.Remove(selected);
        }

        private void IdSPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (idSPTextBox.Text.Length == 0)
            {
                idSPLabel.Visibility = Visibility.Visible;
            }
            else
                idSPLabel.Visibility = Visibility.Hidden;
        }

        private void ProductInfoEdit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(filename);
                image.EndInit();

                ProductImage.Height = 0;
                ProductImage.Visibility = Visibility.Hidden;
                ProductImageTemp.Visibility = Visibility.Visible;

                ProductImageTemp.Source = image;
            }
        }
        private void UndoImageProductInfomation()
        {
            if (ProductImage.Height == 0)
            {
                ProductImage.Visibility = Visibility.Visible;
                ProductImageTemp.Visibility = Visibility.Hidden;
                ProductImage.Height = Double.NaN;
                ProductImageTemp.Source = null;
            }
        }
        private void ProductInfoTempEdit_Click(object sender, RoutedEventArgs e)
        {
            UndoImageProductInfomation();
        }
        private int FindIndexCategory(int ID)
        {
            var selected = categories.Where(p => p.ID == ID).FirstOrDefault() as Category;
            if (selected != null)
            {
                return categories.IndexOf(selected);
            }
            else
            {
                return -1;
            }
        }
        private void ProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UndoImageProductInfomation();
            if (productsListView.SelectedItem != null)
            {
                int selected = (int)productsListView.SelectedItem.GetType().GetProperty("Cat")
                    .GetValue(productsListView.SelectedItem, null);

                CategoryProductCombobox.SelectedIndex = FindIndexCategory(selected);
            }
        }

        private void FilterSetting_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FilterSettingWIndow(_numberOfPage, _priceRange);
            screen.Handler += Screen_Handler;
            screen.ShowDialog();
        }

        private void Screen_Handler(int ProductQuantites, int PriceRanges)
        {
            _numberOfPage = ProductQuantites;
            _priceRange = PriceRanges;

            UpdateData(categoriesComboBox.SelectedItem as Category);
            Paging(categoriesComboBox.SelectedItem as Category);
        }
    }
}
