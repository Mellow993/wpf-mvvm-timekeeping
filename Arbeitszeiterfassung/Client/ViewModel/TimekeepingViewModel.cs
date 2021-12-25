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
        private static readonly WorkTimeMeasurementModel _workTimeMeasurementModel = new WorkTimeMeasurementModel();
        //private WorkTimeMeasurementModel _workTimeMeasurementModel;
        //public WorkTimeMeasurementModel WorkTimeMeasurementModel
        //{
        //    get => _workTimeMeasurementModel;
        //    set => _workTimeMeasurementModel = value;
        //}

        public static WorkTimeMeasurementModel WorkTimeMeasurementModelInstance
        {
            get => _workTimeMeasurementModel;
        }

        public TimekeepingViewModel()
        {
            PrepareCommands();
        }
        public void PrepareCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime);
            _finishWorkCommand = new DelegateCommand(FinishWork);
            _exitWindowCommand = new DelegateCommand(ExitWindow);
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


        #region Methods
        private void StartTimekeeping()
        {
            WorkTimeMeasurementModel getTime = new WorkTimeMeasurementModel();
            MessageBox.Show(getTime.StartWork.ToString());
        }
        
        private void StartBreakTime()
        {
            WorkTimeMeasurementModelInstance.StartWork = DateTime.Now;
            _ = MessageBox.Show(WorkTimeMeasurementModelInstance.StartWork.ToString());
        }

        private void FinishWork()
        {
            MessageBox.Show(WorkTimeMeasurementModelInstance.StartWork.ToString());
        }

        private void ExitWindow()
        {
            Application.Current.Shutdown();
        }
        #endregion

    }
}
