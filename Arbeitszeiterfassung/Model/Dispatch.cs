using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Arbeitszeiterfassung.Model
{
    public class Dispatch : EventArgs
    {
        private readonly NotifyUser _notifyuser = new NotifyUser();
        private readonly string _info = "Hinweis";
        private readonly string _warning = "Warnung";
        private readonly string _error = "Fehler";
        private readonly string _startWork = "Arbeitsbeginn";
        private readonly string _startBreak = "Pause";
        private readonly string _continueWork = "Weiterarbeiten";
        private readonly string _saveTime = "Arbeitszeit wurde gespeichert";
        private readonly string _noSaveTime = "Arbeitszeit konnte nicht gespeichert werden";
        private readonly string _finishWork = "Arbeitsende";
        private readonly string _serviceTimes = "Beachten Sie die Servicezeiten";
        private readonly ToolTipIcon __info = ToolTipIcon.Info;
        private readonly ToolTipIcon __warning = ToolTipIcon.Warning;
        private readonly ToolTipIcon __error = ToolTipIcon.Error;


        public void StartWorking(object sender, Dispatch e) => _notifyuser.ShowInformation(_info, _startWork, __info);
        public void StartBreak(object sender, Dispatch e) => _notifyuser.ShowInformation(_info, _startBreak, __info);
        public void ContinueWork(object sender, Dispatch e) => _notifyuser.ShowInformation(_info, _continueWork, __info);
        public void FinishWorking(object sender, Dispatch e) => _notifyuser.ShowInformation(_info, _finishWork, __info);
        public void SaveTimes(object sender, Dispatch e) => _notifyuser.ShowInformation(_info, _saveTime, __info);
        public void NoSaveTimes(object sender, Dispatch e) => _notifyuser.ShowInformation(_error, _noSaveTime, __error);
        public void ServiceTimes(object sender, Dispatch e) => _notifyuser.ShowInformation(_warning, _serviceTimes, __warning);
    }

    public class NotifyUser : ObservableRecipient 
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly int _timeOfAppearance = 5000;
        private readonly string _pathToIcon = @"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico";
        private readonly string _nameInTraybar = "Arbeitszeiterfassung";
        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest
        {
            get => _notifyRequest;
            set => SetProperty(ref _notifyRequest, value);
        }
        public NotifyUser()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon(_pathToIcon);
            _notifyIcon.Visible = true;
            _notifyIcon.Text = _nameInTraybar;
        }
        public void ShowInformation(string infotype, string info, ToolTipIcon mode)
            =>  _notifyIcon.ShowBalloonTip(_timeOfAppearance, infotype, info, mode);
    }
}
