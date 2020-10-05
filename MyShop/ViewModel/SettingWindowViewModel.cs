using System.Windows.Input;
using System.Windows;
using MyShop.View;

namespace MyShop.ViewModel
{
    public class SettingWindowViewModel : BaseViewModel
    {
        string server = "";
        string db = "";
        #region command
        public ICommand ServernameTextBoxGotFocus { get; set; }
        public ICommand DBNameTextBoxGotFocus { get; set; }
        public ICommand ServernameTextBoxLostFocus { get; set; }
        public ICommand DBNameTextBoxLostFocus { get; set; }
        public ICommand SaveButtonClick { get; set; }
        public ICommand CancelButtonClick { get; set; }
        public ICommand LoadCommand { get; set; }
        #endregion
        public SettingWindowViewModel()
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
                        MessageBox.Show("CLosed");
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
                    }
                );
        }
        
    }
}
