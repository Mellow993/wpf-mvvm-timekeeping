using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Form = System.Windows.Forms;

namespace Arbeitszeiterfassung.Client.ViewModel
{

    class TimekeepingViewModel : ObservableRecipient // ViewModelBase
    {
        
        public event EventHandler<Dispatch> OnWorkStarted;
        public event EventHandler<Dispatch> OnBreakStarted;
        public event EventHandler<Dispatch> OnContinueWorkd;
        public event EventHandler<Dispatch> OnWorkFinished;
        public event EventHandler<Dispatch> OnSave;
        public event EventHandler<Dispatch> OnNoSave;
        public event EventHandler<Dispatch> OnServiceTime;

        readonly ButtonControl bc = new ButtonControl();

        #region Fields and properties
        #region Hide form in traybar

        private readonly Dispatch _dispatch;

        private readonly Form.NotifyIcon _notifyIcon;
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest { get => _notifyRequest; set => SetProperty(ref _notifyRequest, value); }
        public static WorkTimeMeasurementModel WorkTimeMeasurementModelInstance { get; } = new WorkTimeMeasurementModel();


        private bool _showInTaskbar;
        private WindowState _windowState;
        public bool ShowInTaskbar { get => _showInTaskbar; set => SetProperty(ref _showInTaskbar, value); }
        public WindowState WindowState { get => _windowState;
            set
            {
                ShowInTaskbar = true;
                SetProperty(ref _windowState, value);
                ShowInTaskbar = value != WindowState.Minimized;
            }
        }
        #endregion

        private string _destination;
        public string Destination { get => _destination;
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
            _dispatch= new Dispatch();
            bc.CurrentState = ButtonControl.State.None;
            SetupCommands();
        }
        #endregion

        #region Setup commands
        private void SetupCommands()// preapare commands and user settings
        {
            ModelCommands();        // interact with the model
            ControlCommands();      // commands for save and close
            FetchUserSettings();    // get or sets registry key
            SubscribeToEvents();       // subscribe for events => not finished
        }

        private void ModelCommands()
        {
            _startTimekeepingCommand = new DelegateCommand(StartTimekeeping, CanStartTimeKeeping);
            _startBreakTimeCommand = new DelegateCommand(StartBreakTime, CanDoBreak);
            _continueWorkCommand = new DelegateCommand(ContinueWork, CanContinueWork);
            _finishWorkCommand = new DelegateCommand(FinishWork, CanFinishWork);
        }     

        private void ControlCommands()
        {
            _saveCommand = new DelegateCommand(SaveInformations, CanSave);
            _exitWindowCommand = new DelegateCommand(ExitWindow);
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Minimized; });
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
        }  
        private void FetchUserSettings()
        {
            UserSettings usersettings = new UserSettings();
            Destination = usersettings.ReadRegistry();
            OnPropertyChanged(nameof(Destination));
        } 
        private void SubscribeToEvents()       
        {
            OnWorkStarted += _dispatch.StartWorking;
            OnBreakStarted += _dispatch.StartBreak;
            OnContinueWorkd += _dispatch.ContinueWork;
            OnWorkFinished += _dispatch.FinishWorking;
            OnSave += _dispatch.SaveTimes;
            OnNoSave += _dispatch.NoSaveTimes;
            OnServiceTime += _dispatch.ServiceTimes;
        }
        #endregion

        #region Commands to start the logic part
        private void StartTimekeeping()
        {
            bc.CurrentState = ButtonControl.State.Work;
            WorkTimeMeasurementModelInstance.StartWork = GetDateTime();
            if (!Validation.IsServiceTime(WorkTimeMeasurementModelInstance.StartWork, WorkTimeMeasurementModelInstance.LongDay))
                _notifyIcon.ShowBalloonTip(5000, "Hinweis", "Servicezeiten beachten!", Form.ToolTipIcon.Info);
            
            RaiseStart("Arbeit beginnt");
            RaisePropertyChanged();
        }
        private bool CanStartTimeKeeping() => bc.CurrentState == ButtonControl.State.None ? true : false;
        private void StartBreakTime()
        {
            bc.CurrentState = ButtonControl.State.Break;
            WorkTimeMeasurementModelInstance.StartBreak = GetDateTime();
            RaisePropertyChanged();
        }
        private bool CanDoBreak() => (bc.CurrentState == ButtonControl.State.Work) ? true : false;
        private void ContinueWork()
        { 
            WorkTimeMeasurementModelInstance.ContinueWork = GetDateTime();
            bc.CurrentState = ButtonControl.State.ContinueWork;
            RaisePropertyChanged();
        }
        private bool CanContinueWork() => (bc.CurrentState == ButtonControl.State.Break) ? true : false;
        private void FinishWork()
        {
            _notifyIcon.ShowBalloonTip(10000, "Hinweis", "Feierabend", Form.ToolTipIcon.Info);
            WorkTimeMeasurementModelInstance.FinishWork = GetDateTime();
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
            bc.CurrentState = ButtonControl.State.HomeTime;
            RaiseFinish("Feierabend");
            RaisePropertyChanged();
        }
        private bool CanFinishWork() => bc.CurrentState != ButtonControl.State.Break && bc.CurrentState != ButtonControl.State.None ? true : false;
        private void SaveInformations()
        {
            var initialDirectory = @"C:\Users\Lenovo\Desktop";
            var allowedFiles = "Text file (*.txt)|*.txt";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = initialDirectory;
            saveFileDialog.Filter = allowedFiles;

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
        private bool CanSave() => bc.CurrentState == ButtonControl.State.HomeTime ? true : false;
        private void ExitWindow()
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        #endregion

        #region Provide acutal date and time
        private DateTime GetDateTime() => DateTime.Now;
        #endregion

        private void RaisePropertyChanged([CallerMemberName] string propname = "")
        {
            ((DelegateCommand)StartTimekeepingCommand).OnExecuteChanged();
            ((DelegateCommand)StartBreakTimeCommand).OnExecuteChanged();
            ((DelegateCommand)ContinueWorkCommand).OnExecuteChanged();
            ((DelegateCommand)FinishWorkCommand).OnExecuteChanged();
            ((DelegateCommand)SaveCommand).OnExecuteChanged();
        }
        #region methods waiting for event
        public void RaiseStart(string message) => OnWorkStarted?.Invoke(this, new Dispatch());
        public void RasieBreak(string message) => OnBreakStarted?.Invoke(this, new Dispatch());
        public void RasieContinue(string message) => OnContinueWorkd?.Invoke(this, new Dispatch());
        public void RaiseFinish(string message) => OnWorkFinished?.Invoke(this, new Dispatch());
        public void RaiseServiceTime(string message) => OnServiceTime?.Invoke(this, new Dispatch());
        public void RaiseSave(string message) => OnSave?.Invoke(this, new Dispatch());
        public void RaiseNoSave(string message) => OnNoSave?.Invoke(this, new Dispatch());


        #endregion
    }
}
