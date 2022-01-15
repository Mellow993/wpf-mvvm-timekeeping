using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Forms;


namespace Arbeitszeiterfassung.Model
{
    public class Dispatch : EventArgs
    {
        private readonly NotifyUser _notifyuser = new NotifyUser();
        public string Message { get; set; }
        public Dispatch() {}
        public Dispatch(string message) { Message = message; }
        public void StartWorking(object sender, Dispatch e) => _notifyuser.ShowInformation("Hinweis", e.Message, ToolTipIcon.Info);
        public void FinishWorking(object sender, Dispatch e) =>_notifyuser.ShowInformation("Hinweis", e.Message, ToolTipIcon.Info);
        public void Inform(object sender, Dispatch e) => _notifyuser.ShowInformation("Hinweis", e.Message, ToolTipIcon.Info); 
        public void Save(object sender, Dispatch e) => _notifyuser.ShowInformation("Hinweis", e.Message, ToolTipIcon.Info); 
    }

    public class NotifyUser : ObservableRecipient // : Notify
    {
        private readonly NotifyIcon _notifyIcon;

        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest
        {
            get => _notifyRequest;
            set => SetProperty(ref _notifyRequest, value);
        }
        public NotifyUser()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Arbeitszeiterfassung";
        }

        public void ShowInformation(string infotype, string info, ToolTipIcon mode)
        {
            _notifyIcon.ShowBalloonTip(5000, infotype, info, mode);
        }
    }

   
}
