using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Arbeitszeiterfassung.Model;

namespace Arbeitszeiterfassung.Client.ViewModel
{
    class TimekeepingViewModel : ViewModelBase
    {

        private WorkTimeMeasurementModel _workTimeMeasurementModel;
        public WorkTimeMeasurementModel WorkTimeMeasurementModel
        {
            get => _workTimeMeasurementModel;
            set => _workTimeMeasurementModel = value;
        }
        public TimekeepingViewModel()
        {
            //var _workTimeMeasurementModel = new WorkTimeMeasurementModel(); // why I can't initiate an object and use it in the entire class
            PrepareCommands();
        }

        #region Commands
        private DelegateCommand _startTimekeepingCommand;
        private DelegateCommand _startBreakTimeCommand;
        private DelegateCommand _finishWorkCommand;
        private DelegateCommand _exitWindowCommand;
        
        public ICommand StartTimekeepingCommand { get => _startTimekeepingCommand; }
        public ICommand StartBreakTimeCommand { get => _startBreakTimeCommand; }
        public ICommand FinishWorkCommand { get => _finishWorkCommand; }
        public ICommand ExitWindowCommand { get => _exitWindowCommand; }
        #endregion

        public void PrepareCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime);
            _finishWorkCommand = new DelegateCommand(FinishWork);
            _exitWindowCommand = new DelegateCommand(ExitWindow);
        }

        #region Methods
        private void StartTimekeeping()
        {
        }
        
        private void StartBreakTime()
        {
         
            //MessageBox.Show(foo2.FinishWork.ToString());
        }

        private void FinishWork()
        {
            //WorkTimeMeasurementModel foo2 = new WorkTimeMeasurementModel();
        }

        private void ExitWindow()
        {
            Application.Current.Shutdown();
        }
        #endregion

    }
}
