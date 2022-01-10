﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Form = System.Windows.Forms;
using Arbeitszeiterfassung.Client.Common;
using Arbeitszeiterfassung.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Forms;

namespace Arbeitszeiterfassung.Model
{

    public class Notify 
    {
        private NotifyIcon _notifyIcon;
        public Notify() 
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Arbeitszeiterfassung";
        }
    }

    public class UserOutpus : Notify
    {
        public event EventHandler FileHasBeenSaved;
        public event EventHandler Information;
        public event EventHandler Warning;
        public event EventHandler Error;
        public event EventHandler Output;


        public void OnInformation() => Information ?. Invoke(this, EventArgs.Empty);
        public void OnWarning() => Warning ?. Invoke(this, EventArgs.Empty);
        public void OnError() => Error ?. Invoke(this, EventArgs.Empty);  
        public void OnSaveCompleted() => FileHasBeenSaved?.Invoke(this, EventArgs.Empty);
        public void OnUserOutput() => Output?.Invoke(this, EventArgs.Empty); 


        public static void OutputInformation(object sender, EventArgs e)
        {

        }

        public static void UserOutputInformations(object sender, EventArgs e)
        {

        }

        public static void UserOutputWarning(object sender, EventArgs e)
        {

        }

        public static void UserOutputError(object sender, EventArgs e)
        {

        }
    }

}

