using System.Windows.Input;
using System.Windows;

namespace MyShop.ViewModel
{
    public class LoginWindowViewModel : BaseViewModel
    {
        #region command
        public ICommand UsernameTextBoxGotFocus { get; set; }
        public ICommand PasswordPasswordBoxGotFocus { get; set; }
        public ICommand UsernameTextBoxLostFocus { get; set; }
        public ICommand PasswordPasswordBoxLostFocus { get; set; }
        #endregion
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
        }
    }
}
