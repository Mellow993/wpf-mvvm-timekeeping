using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Arbeitszeiterfassung.Client.Common
{
    class DelegateCommand : ICommand
    {
        private Action _execute;
        public DelegateCommand(Action execute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute), "no execution");
            _execute = execute;
        }
        public event EventHandler CanExecuteChanged;
        public void OnExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public void Execute(object parameter) => _execute.Invoke();
        public bool CanExecute(object parameter) => true;
    }
}
