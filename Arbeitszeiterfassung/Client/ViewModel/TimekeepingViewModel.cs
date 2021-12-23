using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Arbeitszeiterfassung.Client.ViewModel
{
    class TimekeepingViewModel : ViewModelBase
    {

        public TimekeepingViewModel()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping);
        }

        private DelegateCommand _startTimekeepingCommand;
        public ICommand StartTimekeepingCommand { get => _startTimekeepingCommand; }

        #region Methods
        private void StartTimekeeping()
        {

        }
        #endregion

    }
}
