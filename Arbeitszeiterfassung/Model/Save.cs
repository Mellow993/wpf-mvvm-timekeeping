using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbeitszeiterfassung.Model;
using System.Diagnostics;
using System.IO;
using Arbeitszeiterfassung.Client.Common.Converters;
using System.Windows;
using Microsoft.Win32;
using Arbeitszeiterfassung.Client.Common;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Form = System.Windows.Forms;
using Arbeitszeiterfassung.Client.ViewModel;

namespace Arbeitszeiterfassung.Model
{
    interface IDestination
    {
        string Destination { get; set; } 
    }

    class Save
    {
        private readonly WorkTimeMeasurementModel _worktime;
        public  WorkTimeMeasurementModel WorkTimeMeasurementModel { get => _worktime; }
        public TimekeepingViewModel TimekeepingViewModel { get; set; }
        private string _initialDirectory = @"C:\";
        private string _allowedFiles = "Text file (*.txt)|*.txt";
        private string _destination;
        private string Destination
        {
            get => TimekeepingViewModel.Destination;
            set => _destination = value;
        }
        private StringBuilder _content;
        public StringBuilder Content
        {
            get => _content;
            set => _content = value;
        }

        public Save(string destination) { _destination = destination; }




        public bool SaveFile()
        {
            SetupSaveWindow();
            DirectoryInfo directoryinfo = new DirectoryInfo(Path.GetDirectoryName(Destination));
            if (directoryinfo.Exists)
            {
                PrepareOutput();
                return true;
            }
            else
                return false;
        }


        private void SetupSaveWindow()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
                _ = saveFileDialog.FileName;
        }

        private void PrepareOutput() => File.AppendAllText(Destination, BuildInformation());
        private string BuildInformation()
        {
            StringBuilder Content = new StringBuilder();
            _ = Content.Append("\nAktueller Tag\t" + WorkTimeMeasurementModel.StartWork.ToShortDateString() + "\n");
            _ = Content.Append("Arbeitsbeginn\t" + WorkTimeMeasurementModel.StartWork.ToShortTimeString() + " Uhr" + "\n");
            _ = Content.Append("Pausenbeginn\t" + WorkTimeMeasurementModel.StartBreak.ToShortTimeString() + " Uhr" + "\n");
            _ = Content.Append("Pausenzeit\t" + WorkTimeMeasurementModel.BreakTime.TotalMinutes.ToString("#") + " Minuten" + "\n");
            _ = Content.Append("Pausenende\t" + WorkTimeMeasurementModel.ContinueWork.ToShortTimeString() + " Uhr" + "\n");
            _ = Content.Append("Feierabend\t" + WorkTimeMeasurementModel.FinishWork.ToShortTimeString() + " Uhr" + "\n"); ;
            _ = Content.Append("Nettozeit\t" + WorkTimeMeasurementModel.NetWorkTime + " (hh:mm)" + "\n");
            _ = Content.Append("Bruttozeit\t" + WorkTimeMeasurementModel.GrossWorkTime + " (hh:mm)" + "\n");
            _ = Content.Append("Timecard\t" + WorkTimeMeasurementModel.Timecard + "\n");
            _ = Content.Append("------------------------------------\n");
            return Content.ToString();
        }
    }
}