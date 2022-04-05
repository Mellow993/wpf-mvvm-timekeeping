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
        #region Fields and properties
        private string _destination;
        readonly ButtonControl bc = new ButtonControl();
        private readonly Dispatch _dispatch;
        private readonly Form.NotifyIcon _notifyIcon;
        private WindowState _windowState;
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest { get => _notifyRequest; set => SetProperty(ref _notifyRequest, value); }
        public static WorkTimeMeasurementModel WorkTimeMeasurementModelInstance { get; } = new WorkTimeMeasurementModel();
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
        public ButtonControl ButtonControl { get; set; }
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                SetProperty(ref _windowState, value);
            }
        }
        #endregion

        #region Declarate Events
        public event EventHandler<Dispatch> OnWorkStarted;
        public event EventHandler<Dispatch> OnBreakStarted;
        public event EventHandler<Dispatch> OnContinueWorkd;
        public event EventHandler<Dispatch> OnWorkFinished;
        public event EventHandler<Dispatch> OnSave;
        public event EventHandler<Dispatch> OnNoSave;
        public event EventHandler<Dispatch> OnServiceTime;
        #endregion

        #region Declarate commands
        public ICommand LoadedCommand { get; private set; }
        public ICommand ClosingCommand { get; private set; }
        public ICommand NotifyCommand { get; private set; }
        public ICommand NotifyIconOpenCommand { get; private set; }
        public ICommand NotifyIconExitCommand { get; private set; }
        public ICommand StartTimekeepingCommand { get; private set; }
        public ICommand StartBreakTimeCommand { get; private set; }
        public ICommand ContinueWorkCommand { get; private set; }
        public ICommand FinishWorkCommand { get; private set; }
        public ICommand ExitWindowCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CorrectionCommand { get; private set; }
        #endregion
        
        #region Constructor
        public TimekeepingViewModel()
        {
            _notifyIcon = new Form.NotifyIcon();
            _dispatch = new Dispatch();
            ButtonControl = new ButtonControl();
            ButtonControl.CurrentState = ButtonControl.State.None;
            SetupCommands();
        }
        #endregion

        #region Provide constructor content
        private void SetupCommands()// preapare commands and user settings
        {
            ModelCommands();        // interact with the model
            ControlCommands();      // commands for save and close
            FetchUserSettings();    // get or sets registry key
            SubscribeToEvents();    // subscribe for events 
        }
        private void ModelCommands()
        {
            StartTimekeepingCommand = new DelegateCommand(StartTimekeeping, CanStartTimeKeeping);
            StartBreakTimeCommand = new DelegateCommand(StartBreakTime, CanDoBreak);
            ContinueWorkCommand = new DelegateCommand(ContinueWork, CanContinueWork);
            FinishWorkCommand = new DelegateCommand(FinishWork, CanFinishWork);
            CorrectionCommand = new DelegateCommand(CorrectInput, CanCorrect);
        }    
        private void ControlCommands()
        {
            SaveCommand = new DelegateCommand(SaveInformations, CanSave);
            ExitWindowCommand = new DelegateCommand(ExitWindow);
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Minimized; });
            NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
        }  
        private void FetchUserSettings()
        {
            UserSettings usersettings = new();
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
        private void CorrectInput()
        {
            throw new NotImplementedException();
        }

        private void StartTimekeeping()
        {
            ButtonControl.CurrentState = ButtonControl.State.Work;
            WorkTimeMeasurementModelInstance.StartWork = DateTime.Now;
            if (!WorkTimeMeasurementModelInstance.InServiceTime)
                OnServiceTime?.Invoke(this, new Dispatch());

            OnWorkStarted?.Invoke(this, new Dispatch());
            RaisePropertyChanged();
        }
        private void StartBreakTime()
        {
            ButtonControl.CurrentState = ButtonControl.State.Break;
            WorkTimeMeasurementModelInstance.StartBreak = DateTime.Now;
            OnBreakStarted?.Invoke(this, new Dispatch());
            RaisePropertyChanged();
        }
        private void ContinueWork()
        { 
            WorkTimeMeasurementModelInstance.ContinueWork = DateTime.Now;
            ButtonControl.CurrentState = ButtonControl.State.ContinueWork;
            OnContinueWorkd?.Invoke(this, new Dispatch());
            RaisePropertyChanged();
        }
        private void FinishWork()
        {
            WorkTimeMeasurementModelInstance.FinishWork = DateTime.Now;
            WorkTimeMeasurementModelInstance.CalculateTimeSpan();
            //ButtonControl.CurrentState = ButtonControl.State.HomeTime;
            ButtonControl.CurrentState = ButtonControl.State.None;

            OnWorkFinished?.Invoke(this, new Dispatch());
            RaisePropertyChanged();
        }
        private void SaveInformations() //TODO: Reduce lines
        {
            var initialDirectory = Environment.SpecialFolder.MyDocuments; //@"C:\Users\Lenovo\Desktop";
            var allowedFiles = "Text file (*.txt)|*.txt";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = initialDirectory.ToString();
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
                    OnSave?.Invoke(this, new Dispatch());
                    OnPropertyChanged(nameof(Destination));
                }
            }
            else
                OnNoSave?.Invoke(this, new Dispatch());
        }
        private void ExitWindow()
        {
            _notifyIcon.Dispose();
            Application.Current.Shutdown();
        }
        #endregion


        #region Evaluate execution
        private bool CanStartTimeKeeping()
            => ButtonControl.CurrentState == ButtonControl.State.None ? true : false;
        private bool CanDoBreak()
            => ButtonControl.CurrentState == ButtonControl.State.Work ? true : false;
        private bool CanContinueWork()
            => ButtonControl.CurrentState == ButtonControl.State.Break ? true : false;
        private bool CanFinishWork() 
           => ButtonControl.CurrentState != ButtonControl.State.Break && ButtonControl.CurrentState != ButtonControl.State.None ? true : false;
        private bool CanSave()
            => ButtonControl.CurrentState == ButtonControl.State.HomeTime ? true : false;
        private bool CanCorrect() => true;
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
        #endregion
    }
}
