using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Fluent;
using MyShop.View;
using MyShop.UC;

namespace MyShop.ViewModel
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        #region command
        public ICommand AdminDashboardLoaded { get; set; }
        #endregion
        public AdminDashboardViewModel()
        {
            AdminDashboardLoaded = new RelayCommand<AdminDashboardWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {

                    }
                );
        }
    }
}
