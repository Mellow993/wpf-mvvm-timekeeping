using System;
using System.Windows.Input;

namespace Arbeitszeiterfassung.Client.Common
{
    class DelegateCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action execute) { _execute = execute; }
        public DelegateCommand(Action execute, Func<bool> canexecute) { _execute = execute; _canExecute = canexecute; }
        public void OnExecuteChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());
        public void Execute(object parameter) => _execute(); 
        public bool CanExecute(object parameter) => _canExecute != null ? _canExecute() : true;
    }
}
