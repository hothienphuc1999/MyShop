using Fluent;
using MyShop.ViewModel;

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
            this.DataContext = ViewModel = new AdminDashboardViewModel();
        }
    }
}
