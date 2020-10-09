using System.Windows.Input;
using System.Windows;
using MyShop.View;
using System.Reflection;
using System.Configuration;

namespace MyShop.ViewModel
{
    public class SettingWindowViewModel : BaseViewModel
    {
        #region command
        public ICommand ServernameTextBoxGotFocus { get; set; }
        public ICommand DBNameTextBoxGotFocus { get; set; }
        public ICommand ServernameTextBoxLostFocus { get; set; }
        public ICommand DBNameTextBoxLostFocus { get; set; }
        public ICommand SaveButtonClick { get; set; }
        public ICommand CancelButtonClick { get; set; }
        public ICommand LoadCommand { get; set; }
        #endregion
        
        public SettingWindowViewModel(string server, string db)
        {
            ServernameTextBoxGotFocus = new RelayCommand<SettingWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        p.servernameLable.Visibility = Visibility.Hidden;
                    }
                );
            DBNameTextBoxGotFocus = new RelayCommand<SettingWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        p.dbnameLable.Visibility = Visibility.Hidden;
                    }
                );
            ServernameTextBoxLostFocus = new RelayCommand<SettingWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        if (p.servernameTextBox.Text.Length == 0)
                        {
                            p.servernameLable.Visibility = Visibility.Visible;
                        }
                    }
                );
            DBNameTextBoxLostFocus = new RelayCommand<SettingWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) => {
                        if (p.dbnameTextBox.Text.Length == 0)
                        {
                            p.dbnameLable.Visibility = Visibility.Visible;
                        }
                    }
                );
            SaveButtonClick = new RelayCommand<SettingWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) =>
                    {
                        LoginWindowViewModel.Server = p.servernameTextBox.Text;
                        LoginWindowViewModel.DB = p.dbnameTextBox.Text;

                        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["server"].Value = p.servernameTextBox.Text;
                        config.AppSettings.Settings["database"].Value = p.dbnameTextBox.Text;
                        config.Save(ConfigurationSaveMode.Minimal);

                        MessageBox.Show("Setting saved!","Notification",MessageBoxButton.OK);
                        p.DialogResult = true;
                    }
                );
            CancelButtonClick = new RelayCommand<SettingWindow>
                (
                    (p) => { return p == null ? false : true; },
                    (p) =>
                    {
                        p.Close();
                    }
                );
            LoadCommand = new RelayCommand<SettingWindow>
            (
                (p) => { return p == null ? false : true; },
                (p) =>
                {
                    p.servernameTextBox.Text = server;
                    p.dbnameTextBox.Text = db;
                    string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    p.versionTextBlock.Text = $"Version {version}";
                }
            );
        }
        
    }
}
