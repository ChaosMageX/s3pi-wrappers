using System;
using System.Windows.Input;

namespace FootprintViewer.Commands
{
    class UserCommand<T> : ICommand
    {
        private readonly Predicate<T> mCanExecute;
        private readonly Action<T> mExecute;

        public UserCommand(Predicate<T> canExecute, Action<T> execute)
        {
            mCanExecute = canExecute;
            mExecute = execute;
        }


        public bool CanExecute(object parameter)
        {
            return mCanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            mExecute((T)parameter);
        }
    }
    
}