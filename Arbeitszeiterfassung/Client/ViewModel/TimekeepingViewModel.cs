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
        #region Declarate Events
        public event EventHandler<Dispatch> OnWorkStarted;
        public event EventHandler<Dispatch> OnBreakStarted;
        public event EventHandler<Dispatch> OnContinueWorkd;
        public event EventHandler<Dispatch> OnWorkFinished;
        public event EventHandler<Dispatch> OnSave;
        public event EventHandler<Dispatch> OnNoSave;
        public event EventHandler<Dispatch> OnServiceTime;

        #endregion


        #region Fields and properties
        readonly ButtonControl bc = new ButtonControl();
        #region Hide form in traybar

        private readonly Dispatch _dispatch;

        private readonly Form.NotifyIcon _notifyIcon;
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest { get => _notifyRequest; set => SetProperty(ref _notifyRequest, value); }
        public static WorkTimeMeasurementModel WorkTimeMeasurementModelInstance { get; } = new WorkTimeMeasurementModel();


        private bool _showInTaskbar;
        public bool ShowInTaskbar { get => _showInTaskbar; set => SetProperty(ref _showInTaskbar, value); }

        private WindowState _windowState;
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


        #region Declarate commands
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
            _dispatch = new Dispatch();
            SetupButtonState();
            SetupCommands();
        }
        #endregion


        #region Provide constructor content
        private void SetupButtonState() => bc.CurrentState = ButtonControl.State.None;
        private void SetupCommands()// preapare commands and user settings
        {
            ModelCommands();        // interact with the model
            ControlCommands();      // commands for save and close
            FetchUserSettings();    // get or sets registry key
            SubscribeToEvents();       // subscribe for events 
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


        #region Button methods
        private void StartTimekeeping()
        {
            bc.CurrentState = ButtonControl.State.Work;
            WorkTimeMeasurementModelInstance.StartWork = DateTime.Now;
            if (!Validation.IsServiceTime(WorkTimeMeasurementModelInstance.StartWork, WorkTimeMeasurementModelInstance.LongDay))
                RaiseServiceTime();

            RaiseStart();
            RaisePropertyChanged();
        }
        private void StartBreakTime()
        {
            bc.CurrentState = ButtonControl.State.Break;
            WorkTimeMeasurementModelInstance.StartBreak = DateTime.Now;
            RasieBreak();
            RaisePropertyChanged();
        }
        private void ContinueWork()
        { 
            WorkTimeMeasurementModelInstance.ContinueWork = DateTime.Now;
            bc.CurrentState = ButtonControl.State.ContinueWork;
            RasieContinue();
            RaisePropertyChanged();
        }
        private void FinishWork()
        {
            WorkTimeMeasurementModelInstance.FinishWork = DateTime.Now;
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
            bc.CurrentState = ButtonControl.State.HomeTime;
            RaiseFinish();
            RaisePropertyChanged();
        }
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
                UserSettings su = new UserSettings(saveFileDialog.FileName);
                su.SetRegistry();
                if (saveTimeKeeping.SaveFile())
                {
                    RaiseSave();
                    OnPropertyChanged(nameof(Destination));
                }
            }
            else
                RaiseNoSave();
        }
        private void ExitWindow()
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        #endregion


        #region Evaluate execution
        private bool CanStartTimeKeeping()
            => bc.CurrentState == ButtonControl.State.None ? true : false;
        private bool CanDoBreak()
            => bc.CurrentState == ButtonControl.State.Work ? true : false;
        private bool CanContinueWork()
            => bc.CurrentState == ButtonControl.State.Break ? true : false;
        private bool CanFinishWork() 
           => bc.CurrentState != ButtonControl.State.Break && bc.CurrentState != ButtonControl.State.None ? true : false;
        private bool CanSave()
            => bc.CurrentState == ButtonControl.State.HomeTime ? true : false;
        #endregion


        #region Raise events
        private void RaisePropertyChanged([CallerMemberName] string propname = "")
        {
            ((DelegateCommand)StartTimekeepingCommand).OnExecuteChanged();
            ((DelegateCommand)StartBreakTimeCommand).OnExecuteChanged();
            ((DelegateCommand)ContinueWorkCommand).OnExecuteChanged();
            ((DelegateCommand)FinishWorkCommand).OnExecuteChanged();
            ((DelegateCommand)SaveCommand).OnExecuteChanged();
        }
        #region methods waiting for event
        public void RaiseStart() => OnWorkStarted?.Invoke(this, new Dispatch());
        public void RasieBreak() => OnBreakStarted?.Invoke(this, new Dispatch());
        public void RasieContinue() => OnContinueWorkd?.Invoke(this, new Dispatch());
        public void RaiseFinish() => OnWorkFinished?.Invoke(this, new Dispatch());
        public void RaiseServiceTime() => OnServiceTime?.Invoke(this, new Dispatch());
        public void RaiseSave() => OnSave?.Invoke(this, new Dispatch());
        public void RaiseNoSave() => OnNoSave?.Invoke(this, new Dispatch());

        #endregion
        #endregion
    }
}
