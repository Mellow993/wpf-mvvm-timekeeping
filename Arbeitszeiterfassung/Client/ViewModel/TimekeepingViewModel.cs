using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Form = System.Windows.Forms;
using System.Drawing;
using Arbeitszeiterfassung.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
//using SystemTrayApp.WPF;
using System.ComponentModel;

namespace Arbeitszeiterfassung.Client.ViewModel
{
    class TimekeepingViewModel : ObservableRecipient // ViewModelBase
    {
        #region Fields and properties
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;

        private void Notify(string message)
        {
            NotifyRequest = new NotifyIconWrapper.NotifyRequestRecord
            {
                Title = "Notify",
                Text = message,
                Duration = 1000
            };
        }
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest
        {
            get => _notifyRequest;
            set => SetProperty(ref _notifyRequest, value);
        }

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

        private string _destination;
        public string Destination
        {
            get => _destination;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _destination = value;
            }
        }
        #endregion

        #region Constructor
        public TimekeepingViewModel() 
        {
            _notifyIcon = new Form.NotifyIcon();
            SetupCommands();
        }
        #endregion

        #region Events

        #endregion

        #region Setup commands
        private void SetupCommands()
        {
            SetupNotification();
            LogicCommands();
            ControlCommands();
        }
        private void SetupNotification()
        {
            _notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Arbeitszeiterfassung";
            //_notifyIcon.Click += OpenItemOnClick;
        }
        private void LogicCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping);
            _startCoffeeBreakCommand = new DelegateCommand(StartCoffeeBreak);
            _finishCoffeeBreakCommand = new DelegateCommand(FinishCoffeeBreak);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime);
            _continueWorkCommand = new DelegateCommand(ContinueWork);
            _finishWorkCommand = new DelegateCommand(FinishWork);
        }
        private void ControlCommands()
        {
            _saveCommand = new DelegateCommand(SaveInformations);
            _exitWindowCommand = new DelegateCommand(ExitWindow);

            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Minimized; });
            //LoadedCommand = new RelayCommand(Loaded);
            //ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);
            //NotifyCommand = new RelayCommand(() => Notify("Hello world!"));
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
            NotifyIconExitCommand = new RelayCommand(() => { Application.Current.Shutdown(); });
        }
        #endregion

        #region Commands
        private DelegateCommand _startCoffeeBreakCommand;
        private DelegateCommand _finishCoffeeBreakCommand;
        private DelegateCommand _startTimekeepingCommand;
        private DelegateCommand _startBreakTimeCommand;
        private DelegateCommand _finishWorkCommand;
        private DelegateCommand _exitWindowCommand;
        private DelegateCommand _continueWorkCommand;
        private DelegateCommand _saveCommand;

        public ICommand StartCoffeeBreakCommand { get =>_startCoffeeBreakCommand; }
        public ICommand FinishCoffeeBreakCommand { get => _finishCoffeeBreakCommand; }
        public ICommand LoadedCommand { get; set; }
        public ICommand ClosingCommand { get; set; }
        public ICommand NotifyCommand { get; set; }
        public ICommand NotifyIconOpenCommand { get; set; }
        public ICommand NotifyIconExitCommand { get; set; }
        public ICommand StartTimekeepingCommand { get => _startTimekeepingCommand; }
        public ICommand StartBreakTimeCommand { get => _startBreakTimeCommand; }
        public ICommand ContinueWorkCommand { get => _continueWorkCommand; }
        public ICommand FinishWorkCommand { get => _finishWorkCommand; }
        public ICommand ExitWindowCommand { get => _exitWindowCommand; }
        public ICommand SaveCommand { get => _saveCommand; }
        #endregion

        #region private methods

        private void FinishCoffeeBreak()
        {

        }

        private void StartCoffeeBreak()
        {

        }


        private void Loaded()
        {
            WindowState = WindowState.Minimized;
        }

        private bool IsEnabledButton()
        {
            return false;
        }
        private void StartTimekeeping()
        {
            WorkTimeMeasurementModelInstance.StartWork = GetDateTime();
            // TODO: Creates new objects when Property is invoked => baaad
            if (!Validation.IsServiceTime(WorkTimeMeasurementModelInstance.StartWork, WorkTimeMeasurementModelInstance.LongDay)) // remove exclamation mark just for debugging
                _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Servicezeiten beachten!", Form.ToolTipIcon.Info);
        }
        private void StartBreakTime() => WorkTimeMeasurementModelInstance.StartBreak = GetDateTime();
        private void ContinueWork() => WorkTimeMeasurementModelInstance.ContinueWork = GetDateTime();
        private void FinishWork()
        {
            WorkTimeMeasurementModelInstance.FinishWork = GetDateTime();
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
        }
        
        private void SaveInformations()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\Users\Lenovo\Desktop";
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                _ = saveFileDialog.FileName;

            Save saveTimeKeeping = new Save(WorkTimeMeasurementModelInstance, saveFileDialog.FileName);
            if (!String.IsNullOrEmpty(saveFileDialog.FileName))
            {
                Destination = saveFileDialog.FileName;
                if (saveTimeKeeping.SaveFile())
                {
                    _notifyIcon.ShowBalloonTip(10000, "Hinweis", "Arbeitszeiten wurden gespeichert", Form.ToolTipIcon.Info);
                    OnPropertyChanged(nameof(Destination));
                }
            }
            else
                _notifyIcon.ShowBalloonTip(10000, "Hinweis", "Arbeitszeiten konnte nicht gespeichert werden", Form.ToolTipIcon.Warning);
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
