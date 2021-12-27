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
        public static WorkTimeMeasurementModel WorkTimeMeasurementModelInstance { get; } = new WorkTimeMeasurementModel();

        public TimekeepingViewModel()
        {
            SetupCommands();
        }
        public void SetupCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime);
            _continueWorkCommand = new DelegateCommand(ContinueWork);
            _finishWorkCommand = new DelegateCommand(FinishWork);
            _hideFormCommand = new DelegateCommand(HideForm);
            _saveCommand = new DelegateCommand(Save);
            _exitWindowCommand = new DelegateCommand(ExitWindow);
        }


        #region Commands
        private DelegateCommand _startTimekeepingCommand;
        private DelegateCommand _startBreakTimeCommand;
        private DelegateCommand _finishWorkCommand;
        private DelegateCommand _exitWindowCommand;
        private DelegateCommand _hideFormCommand;
        private DelegateCommand _continueWorkCommand;
        private DelegateCommand _saveCommand;

        public ICommand StartTimekeepingCommand { get => _startTimekeepingCommand; }
        public ICommand StartBreakTimeCommand { get => _startBreakTimeCommand; }
        public ICommand ContinueWorkCommand { get => _continueWorkCommand; }
        public ICommand FinishWorkCommand { get => _finishWorkCommand; }
        public ICommand HideFormCommand { get => _hideFormCommand; }
        public ICommand ExitWindowCommand { get => _exitWindowCommand; }
        public ICommand SaveCommand { get => _saveCommand; }
        #endregion


        #region Methods
        private void StartTimekeeping()
        {
            WorkTimeMeasurementModelInstance.StartWork = DateTime.Now;
        }
        
        private void StartBreakTime()
        {
            WorkTimeMeasurementModelInstance.StartBreak = DateTime.Now;
        }        
        
        private void ContinueWork()
        {
           WorkTimeMeasurementModelInstance.ContinueWork = DateTime.Now;
        }

        private void FinishWork()
        {
            WorkTimeMeasurementModelInstance.FinishWork = DateTime.Now;
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
            var grossTotalTime = WorkTimeMeasurementModelInstance.GrossWorkTime.ToString(@"hh\:mm");
            var netTotalTime = WorkTimeMeasurementModelInstance.NetWorkTime.ToString(@"hh\:mm");
            var breaktime = WorkTimeMeasurementModelInstance.BreakTime.ToString(@"hh\:mm");
            MessageBox.Show("Pausenzeit: " + breaktime);
            MessageBox.Show("Gesamte Arbeitszeit brutto: " + grossTotalTime);
            MessageBox.Show("Gesamte Arbeitszeit netto: " + netTotalTime);
        }

        private void HideForm()
        {
            throw new NotImplementedException();

        }
        private void Save()
        {
            throw new NotImplementedException();
        }

        private void ExitWindow() => Application.Current.Shutdown();
     
        #endregion

    }
}
