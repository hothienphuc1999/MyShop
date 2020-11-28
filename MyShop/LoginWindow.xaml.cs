using System.Windows;
using MyShop.ViewModel;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindowViewModel ViewModel { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new LoginWindowViewModel();
        }
    }
}
