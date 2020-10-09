using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyShop.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RelayCommand<T> : ICommand
    {

        private readonly Predicate<T> _canExcute; // Lưu trữ điều kiện để thực hiện command- Thực hiện hàm ủy thác
        private readonly Action<T> _execute; // Lưu trữ hàm ủy thác làm việc gì đó

        // Khi khởi tạo thì truyền điều kiện ủy thác và hàm ủy thác
        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("excute null");
            _canExcute = canExecute;
            _execute = execute;
        }
        //Điều kiện để chạy command

        public bool CanExecute(object parameter)
        {
            return _canExcute == null ? true : _canExcute((T)parameter);
        }
        //Hàm ủy thác khi gọi command
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            //Thêm bớt vào cái manager
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
