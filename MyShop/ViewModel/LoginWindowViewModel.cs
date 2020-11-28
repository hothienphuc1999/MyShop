using System.Windows.Input;
using System.Windows;
using System.Configuration;
using MyShop.View;

namespace MyShop.ViewModel
{
    public class LoginWindowViewModel : BaseViewModel
    {
        #region command
        public ICommand UsernameTextBoxGotFocus { get; set; }
        public ICommand PasswordPasswordBoxGotFocus { get; set; }
        public ICommand UsernameTextBoxLostFocus { get; set; }
        public ICommand PasswordPasswordBoxLostFocus { get; set; }
        public ICommand LoginButtonClick { get; set; }
        public ICommand SettingButtonClick { get; set; }
        #endregion
        public static string Server = ConfigurationManager.AppSettings["server"];
        public static string DB = ConfigurationManager.AppSettings["database"];

        public LoginWindowViewModel()
        {
            
            UsernameTextBoxGotFocus = new RelayCommand<LoginWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        p.usernameLable.Visibility = Visibility.Hidden;
                    }
                );
            UsernameTextBoxLostFocus = new RelayCommand<LoginWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        if (p.usernameTextBox.Text.Length == 0)
                        {
                            p.usernameLable.Visibility = Visibility.Visible;
                        }
                    }
                );
            PasswordPasswordBoxGotFocus = new RelayCommand<LoginWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        p.passwordLabel.Visibility = Visibility.Hidden;
                    }
                );
            PasswordPasswordBoxLostFocus = new RelayCommand<LoginWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        if (p.passwordPasswordBox.Password.Length == 0)
                        {
                            p.passwordLabel.Visibility = Visibility.Visible;
                        }
                    }
                );
            LoginButtonClick = new RelayCommand<LoginWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        var screen = new AdminDashboardWindow();
                        //p.Hide();
                        screen.ShowDialog();
                    }
                );
            SettingButtonClick = new RelayCommand<LoginWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        Window window = new SettingWindow(Server,DB);
                        if (window.ShowDialog() == true)
                        {

                        }
                    }
                );

        }
        
    }
}
