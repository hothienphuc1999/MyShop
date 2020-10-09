﻿using Fluent;
using MyShop.ViewModel;
using MyShop.UC;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MyShop.View
{
    /// <summary>
    /// Interaction logic for AdminDashboardWindow.xaml
    /// </summary>
    public partial class AdminDashboardWindow : RibbonWindow
    {
        public AdminDashboardViewModel ViewModel { get; set; }
        public AdminDashboardWindow()
        {
            InitializeComponent();
            // this.DataContext = ViewModel = new AdminDashboardViewModel();
            var screens = new ObservableCollection<TabItem>()
             {
                new TabItem() { Content = new MasterDataUserControl()},
                new TabItem() { Content = new SaleUserControl()},
                new TabItem() { Content = new ReportUserControl()}
            };
            tabs.ItemsSource = screens;

        }
    }
}
