using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Form =  System.Windows.Forms;
using System.Drawing;
using Arbeitszeiterfassung.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;


namespace Arbeitszeiterfassung.Client.ViewModel
{
    class TimekeepingViewModel : ObservableRecipient // ViewModelBase
    {
        private readonly Form.NotifyIcon _notifyIcon;
        public static WorkTimeMeasurementModel WorkTimeMeasurementModelInstance { get; } = new WorkTimeMeasurementModel();

        private bool _showInTaskbar;
        private WindowState _windowState;

        public bool ShowInTaskbar
        {
            get => _showInTaskbar;
            set => SetProperty(ref _showInTaskbar, value);
        }

        public WindowState WindowState
        {
            get => _windowState;

            set
            {
                ShowInTaskbar = true;
                SetProperty(ref _windowState, value);
                ShowInTaskbar = value != WindowState.Minimized;
            }
        }


        public TimekeepingViewModel() 
        {
            _notifyIcon = new Form.NotifyIcon();
            SetupCommands();
        }

        private void SetupNotification()
        {
            _notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Arbeitszeiterfassung";
            _notifyIcon.Click += NotifyIconClick;
        }


        private void NotifyIconClick(object sender, EventArgs e)
        {
            MessageBox.Show("Event wurde getriggert");
        }
        private void SetupCommands()
        {
            SetupNotification();
            LogicCommands();
            ControlCommands();
        }
        private void LogicCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime);
            _continueWorkCommand = new DelegateCommand(ContinueWork);
            _finishWorkCommand = new DelegateCommand(FinishWork);
        }

        private void ControlCommands()
        {
            _saveCommand = new DelegateCommand(Save);
            _hideFormCommand = new DelegateCommand(HideForm);
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
        //private DelegateCommand _canCloseCommand;

        public ICommand StartTimekeepingCommand { get => _startTimekeepingCommand; }
        public ICommand StartBreakTimeCommand { get => _startBreakTimeCommand; }
        public ICommand ContinueWorkCommand { get => _continueWorkCommand; }
        public ICommand FinishWorkCommand { get => _finishWorkCommand; }
        public ICommand HideFormCommand { get => _hideFormCommand; }
        public ICommand ExitWindowCommand { get => _exitWindowCommand; }
        public ICommand SaveCommand { get => _saveCommand; }
        #endregion

        #region private methods

        private bool IsEnabledButton()
        {
            return false;
        }
        private void StartTimekeeping()
        {
            WorkTimeMeasurementModelInstance.StartWork = GetDateTime();

            if (!Validation.IsServiceTime(WorkTimeMeasurementModelInstance.StartWork, WorkTimeMeasurementModelInstance.LongDay)) // remove exclamation mark just for debugging
            {
                _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Servicezeiten beachten!", Form.ToolTipIcon.Info);
            }
        }

        // TODO Notification Klasse implementieren


        private void StartBreakTime() => WorkTimeMeasurementModelInstance.StartBreak = GetDateTime();
        
        private void ContinueWork() => WorkTimeMeasurementModelInstance.ContinueWork = GetDateTime();
     
        private void FinishWork()
        {
            WorkTimeMeasurementModelInstance.FinishWork = GetDateTime();
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
        }

        private void HideForm()
        {
            //this.WindowState = System.Windows.WindowState.Minimized;
            //_notifyIcon.Icon = new Icon(@"../42604hourglassnotdone_99029.ico");
            _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Die Anwendung läuft noch", Form.ToolTipIcon.Info);
        }
     
        private void Save()
        {
            _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Arbeitszeiten wurden gespeichert", Form.ToolTipIcon.Info);
        }

        private void ExitWindow()
        {
            _notifyIcon.Dispose();
             Application.Current.Shutdown();
        }


        private DateTime GetDateTime() => DateTime.Now;
        #endregion

    }
}
