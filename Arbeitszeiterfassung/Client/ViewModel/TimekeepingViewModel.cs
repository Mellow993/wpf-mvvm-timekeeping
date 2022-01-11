using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using Form = System.Windows.Forms;

namespace Arbeitszeiterfassung.Client.ViewModel
{

    class TimekeepingViewModel : ObservableRecipient // ViewModelBase
    {
        public ButtonControl ButtonControl { get; set; } = new ButtonControl(); 
        
        private readonly UserOutputs uo = new UserOutputs();

        private UserOutputs _useroutputs;
        public UserOutputs UserOutputs
        {
            get => _useroutputs;
            set => _useroutputs = value;
        }

        #region Fields and properties
        #region MyRegion

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
        #endregion

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

        #region Commands
        private DelegateCommand _startTimekeepingCommand;
        private DelegateCommand _startBreakTimeCommand;
        private DelegateCommand _finishWorkCommand;
        private DelegateCommand _exitWindowCommand;
        private DelegateCommand _continueWorkCommand;
        private DelegateCommand _saveCommand;

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




        #region Constructor
        public TimekeepingViewModel()
        {
            _notifyIcon = new Form.NotifyIcon();
            SetupCommands();
        }
        #endregion

        #region Setup commands
        private void SetupCommands()// preapare commands and user settings
        {
            ModelCommands();        // interact with the model
            ControlCommands();      // commands for save and close
            FetchUserSettings();    // get or sets registry key
            CheckForEvents();       // subscribe for events => not finished
        }

        private void ModelCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping, CanStartTimeKeeping);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime, CanDoBreak);
            _continueWorkCommand = new DelegateCommand(ContinueWork, CanContinueWork);
            _finishWorkCommand = new DelegateCommand(FinishWork, CanFinishWork);
        }     // interact with the model
        private void ControlCommands()
        {
            _saveCommand = new DelegateCommand(SaveInformations, CanSave);
            _exitWindowCommand = new DelegateCommand(ExitWindow);
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Minimized; });
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
        }   // commands for save and close
        private void FetchUserSettings()
        {
            UserSettings usersettings = new UserSettings();
            Destination = usersettings.ReadRegistry();
            OnPropertyChanged(nameof(Destination));
        } // get or sets registry key
        private void CheckForEvents()       // subscribe for events
        {
            // Event              // Methode
            uo.FileHasBeenSaved += UserOutputs.OutputInformation;
            uo.Information += UserOutputs.OutputInformation;
            uo.Warning += UserOutputs.OutputInformation;
            uo.Error += UserOutputs.OutputInformation;
        }   
        #endregion

        #region Commands to start the logic part

        private void StartTimekeeping()
        {
            //UserOutpus ui = new UserOutpus();
            ButtonControl.Work = true;
            OnPropertyChanged(nameof(ButtonControl.Work));
            uo.OnSaveCompleted();
            WorkTimeMeasurementModelInstance.StartWork = GetDateTime();
            if (!Validation.IsServiceTime(WorkTimeMeasurementModelInstance.StartWork, WorkTimeMeasurementModelInstance.LongDay))
                _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Servicezeiten beachten!", Form.ToolTipIcon.Info);
            
        }
        private bool CanStartTimeKeeping() => true;
        private void StartBreakTime() => WorkTimeMeasurementModelInstance.StartBreak = GetDateTime();
        private bool CanDoBreak() => false;
        private void ContinueWork() => WorkTimeMeasurementModelInstance.ContinueWork = GetDateTime();
        private bool CanContinueWork() => false;
        private void FinishWork()
        {
            _notifyIcon.ShowBalloonTip(10000, "Hinweis", "Feierabend", Form.ToolTipIcon.Info);
            WorkTimeMeasurementModelInstance.FinishWork = GetDateTime();
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
        }
        private bool CanFinishWork() => false;
        private void SaveInformations()
        {
            var initialDirectory = @"C:\Users\Lenovo\Desktop";
            var allowdFiles = "Text file (*.txt)|*.txt";
            UserOutputs uo = new UserOutputs();
            //uo.OnSaveCompleted += ProgrammInformation;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = allowdFiles;

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
        private bool CanSave() => false;
        private void ExitWindow()
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        #endregion

        #region Provide acutal date and time
        private DateTime GetDateTime() => DateTime.Now;
        #endregion

        #region methods waiting for event
        public void ProgrammInformation(object sender, EventArgs e)
        {
            MessageBox.Show("save erfolgreich");
        }

        #endregion
    }
}
