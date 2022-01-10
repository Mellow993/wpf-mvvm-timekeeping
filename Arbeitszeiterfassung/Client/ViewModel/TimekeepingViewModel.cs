using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using Form = System.Windows.Forms;
//using SystemTrayApp.WPF;

namespace Arbeitszeiterfassung.Client.ViewModel
{

    class TimekeepingViewModel : ObservableRecipient // ViewModelBase
    {
        UserOutpus uo = new UserOutpus();

        private UserOutpus _useroutputs;
        public UserOutpus UserOutputs
        {
            get => _useroutputs;
            set => _useroutputs = value;
        }

        private bool _btnEnabled;
        public bool btnEnabled
        {
            get { return _btnEnabled; }
            set
            {
                if (_btnEnabled != value)
                {
                    _btnEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Fields and properties
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
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
                {
                    _destination = value;
                    OnPropertyChanged(nameof(Destination));
                }
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

        #region Setup commands
        private void SetupCommands()
        {
            LogicCommands();
            ControlCommands();
            FetchUserSettings();
            CheckForEvents();
        }

        private void CheckForEvents()
        {
            // Event              // Methode
            uo.FileHasBeenSaved += EventTriggered;
        }

        private void EventTriggered(object sender, EventArgs e)
        {
            MessageBox.Show("event has been triggered");
        }

        private void FetchUserSettings()
        {
            UserSettings usersettings = new UserSettings();
            Destination = usersettings.ReadRegistry();
            OnPropertyChanged(nameof(Destination));
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
            _saveCommand = new DelegateCommand(SaveInformations);
            _exitWindowCommand = new DelegateCommand(ExitWindow);
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Minimized; });
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
        }
        #endregion

        #region Commands
        private DelegateCommand _startTimekeepingCommand;
        private DelegateCommand _startBreakTimeCommand;
        private DelegateCommand _finishWorkCommand;
        private DelegateCommand _exitWindowCommand;
        private DelegateCommand _continueWorkCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _addKeyCommand;

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
        public ICommand AddKeyCommand { get => _addKeyCommand; }
        #endregion

        #region private methods
        private void StartTimekeeping()
        {
            //UserOutpus ui = new UserOutpus();
            uo.OnSaveCompleted();
            WorkTimeMeasurementModelInstance.StartWork = GetDateTime();
            // TODO: Creates new objects when Property is invoked => baaad
            if (!Validation.IsServiceTime(WorkTimeMeasurementModelInstance.StartWork, WorkTimeMeasurementModelInstance.LongDay))
            {
                _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Servicezeiten beachten!", Form.ToolTipIcon.Info);
            }
        }
        private void StartBreakTime() => WorkTimeMeasurementModelInstance.StartBreak = GetDateTime();
        private void ContinueWork() => WorkTimeMeasurementModelInstance.ContinueWork = GetDateTime();
        private void FinishWork()
        {
            _notifyIcon.ShowBalloonTip(10000, "Hinweis", "Feierabend", Form.ToolTipIcon.Info);
            WorkTimeMeasurementModelInstance.FinishWork = GetDateTime();
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
        }

        private void SaveInformations()
        {
            UserOutpus uo = new UserOutpus();
            //uo.OnSaveCompleted += ProgrammInformation;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\Users\Lenovo\Desktop";
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
                _ = saveFileDialog.FileName;

            Save saveTimeKeeping = new Save(WorkTimeMeasurementModelInstance, saveFileDialog.FileName);
            if (!String.IsNullOrEmpty(saveFileDialog.FileName))
            {
                Destination = saveFileDialog.FileName;
                UserSettings su = new UserSettings(Destination);
                su.SetRegistry();

                if (saveTimeKeeping.SaveFile())
                {
                    //OnSaveCompleted();
                    //_notifyIcon.ShowBalloonTip(10000, "Hinweis", "Arbeitszeiten wurden gespeichert", Form.ToolTipIcon.Info);
                    OnPropertyChanged(nameof(Destination));
                }
            }
            else
                _notifyIcon.ShowBalloonTip(10000, "Hinweis", "Arbeitszeiten konnte nicht gespeichert werden", Form.ToolTipIcon.Warning);
        }

        public void ProgrammInformation(object sender, EventArgs e)
        {
            MessageBox.Show("save erfolgreich");
        }

        private void ExitWindow()
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        private DateTime GetDateTime() => DateTime.Now;
        private bool startTimekeeping;
        public bool _StartTimekeeping { get => startTimekeeping; set => SetProperty(ref startTimekeeping, value); }

        #endregion
    }
}
