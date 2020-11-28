using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using MyShop.View;
using MyShop.UC;
using Microsoft.Win32;
using System;
using Aspose.Cells;
using System.Diagnostics;
using MyShop.Model;
using System.Collections.Generic;
using System.Windows;
using Image = MyShop.Model.Image;
using System.Windows.Media.Imaging;
using System.IO;

namespace MyShop.ViewModel
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string data)
        {
            bool result = data.Length == 0;
            return result;
        }
    }
    public class AdminDashboardViewModel : BaseViewModel
    {
        #region command
        public ICommand AdminDashboardLoaded { get; set; }
        public ICommand ImportExcelCommand { get; set; }
        public ICommand AddCategoryCommand { get; set; }
        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        #endregion
        public AdminDashboardViewModel()
        {
            AdminDashboardLoaded = new RelayCommand<AdminDashboardWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {

                        var screens = new ObservableCollection<TabItem>()
                        {
                            new TabItem() { Content = new MasterDataUserControl()},
                            new TabItem() { Content = new SaleUserControl()},
                            new TabItem() { Content = new ReportUserControl()}
                        };
                        p.tabs.ItemsSource = screens;
                        p.statusBar.Content = "Ready";
                    }
                );
            ImportExcelCommand = new RelayCommand<AdminDashboardWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        var screen = new OpenFileDialog();
                        if (screen.ShowDialog() == true)
                        {
                            var filename = screen.FileName;
                            string safefilename = screen.SafeFileName;
                            string filedirs = filename.Replace(safefilename, "");
                            string imagedir = filedirs + "images\\";

                            var workbook = new Workbook(filename);
                            var sheets = workbook.Worksheets;

                            MyShopEntities db = new MyShopEntities();
                            foreach (var sheet in sheets)
                            {
                                Category category = new Category
                                {
                                    Name = sheet.Name
                                };
                                db.Categories.Add(category);

                                int row = 3;
                                var cell = sheet.Cells[$"B{row}"];

                                while (cell.Value != null)
                                {
                                    Product product = new Product
                                    {
                                        SKU = sheet.Cells[$"C{row}"].StringValue,
                                        Name = sheet.Cells[$"D{row}"].StringValue,
                                        Price = sheet.Cells[$"E{row}"].IntValue,
                                        Quantity = sheet.Cells[$"F{row}"].IntValue,
                                        Description = sheet.Cells[$"G{row}"].StringValue,
                                        Category = category
                                    };

                                    db.Products.Add(product);
                                   

                                    var imageName = sheet.Cells[$"H{row}"].StringValue;
                                    var imagefulldir = imagedir + imageName;

                                    var image = new BitmapImage(new Uri(imagefulldir, UriKind.Absolute));
                                    var encoder = new JpegBitmapEncoder();
                                    encoder.Frames.Add(BitmapFrame.Create(image));

                                    using (var stream = new MemoryStream())
                                    {
                                        encoder.Save(stream);
                                        Image photo = new Image()
                                        {
                                            Product = product,
                                            Data = stream.ToArray()
                                        };

                                        db.Images.Add(photo);
                                    }
                                    row++;
                                    cell = sheet.Cells[$"B{row}"];
                                }
                            }
                            db.SaveChanges();

                            MessageBox.Show("Import Successfully!");
                        }
                    }
                );
            
            AddCategoryCommand = new RelayCommand<AdminDashboardWindow>
                (
                (p) => { return p == null ? false : true; },
                (p) =>
                {
                    var userControl = p.tabs.SelectedContent as MasterDataUserControl;
                    userControl.HandleParentEvent(
                    MasterDataUserControl.MasterDataAction.AddNewCategory
                    );
                }
                );
            DeleteCategoryCommand = new RelayCommand<AdminDashboardWindow>
                (
                (p) => { return p == null ? false : true; },
                (p) =>
                {
                    var userControl = p.tabs.SelectedContent as MasterDataUserControl;
                    userControl.HandleParentEvent(
                    MasterDataUserControl.MasterDataAction.DeleteSelectedCategory
                    );
                }
                );
            AddProductCommand = new RelayCommand<AdminDashboardWindow>
                (
                (p) => { return p == null ? false : true; },
                (p) =>
                {
                    var userControl = p.tabs.SelectedContent as MasterDataUserControl;
                    userControl.HandleParentEvent(
                    MasterDataUserControl.MasterDataAction.AddNewProduct
                    );
                }
                );
            UpdateProductCommand = new RelayCommand<AdminDashboardWindow>
                (
                (p) => { return p == null ? false : true; },
                (p) =>
                {
                    var userControl = p.tabs.SelectedContent as MasterDataUserControl;
                    userControl.HandleParentEvent(
                    MasterDataUserControl.MasterDataAction.UpdateSelectedProduct
                    );
                }
                );
            DeleteProductCommand = new RelayCommand<AdminDashboardWindow>
                (
                (p) => { return p == null ? false : true; },
                (p) =>
                {
                    var userControl = p.tabs.SelectedContent as MasterDataUserControl;
                    userControl.HandleParentEvent(
                    MasterDataUserControl.MasterDataAction.DeleteSelectedProduct
                    );
                }
                );


        }
}
}
