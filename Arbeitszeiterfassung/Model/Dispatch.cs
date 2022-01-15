using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Form = System.Windows.Forms;



namespace Arbeitszeiterfassung.Model
{
    public class Dispatch : EventArgs
    {
        private string _message = string.Empty;
        public string Message { get => _message; set => _message = value; }
        public Dispatch(string message) 
        {
            _message = message;
        }
        public static void FinishWorking(object sender, Dispatch e)
        {
            var notify = new NotifyUser(e.Message);
            notify.ShowInformation();
        }

        public static void StartWorking(object sender, Dispatch e)
        {
            var notify = new NotifyUser(e.Message);
            notify.ShowInformation();
        }

    }

    public class NotifyUser : ObservableRecipient // : Notify
    {
        private string _message = string.Empty;
        public string Message { get => _message; set => _message = value; }

        private readonly Form.NotifyIcon _notifyIcon;

        private NotifyIconWrapper.NotifyRequestRecord? _notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord? NotifyRequest
        {
            get => _notifyRequest;
            set => SetProperty(ref _notifyRequest, value);
        }

        public NotifyUser(string message)
        {
            _message = message;
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Arbeitszeiterfassung";
        }

        public void ShowInformation()
        {
            _notifyIcon.ShowBalloonTip(10000, "Hinweis", Message, Form.ToolTipIcon.Info);

        }
    }

   
}
