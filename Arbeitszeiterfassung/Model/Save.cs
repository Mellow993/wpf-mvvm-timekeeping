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


namespace Arbeitszeiterfassung.Model
{
    class Save
    {
        #region Fields and Attributes
        private readonly WorkTimeMeasurementModel _workTimeMeasurementModel;

        public WorkTimeMeasurementModel WorkTimeMeasurementModel { get => _workTimeMeasurementModel; }

        private string _destination;

        private string Destination
        {
            get => _destination;
            set => _destination = value;
        }

        private StringBuilder _content;

        public StringBuilder Content
        {
            get => _content;
            set => _content = value;
        }
        #endregion

        #region Constructor
        public Save(WorkTimeMeasurementModel worktimemeasurementmodel, string destination) 
        { 
            _workTimeMeasurementModel = worktimemeasurementmodel;
            _destination = destination;
        }
        #endregion

        #region Public methods (SaveFile)
        public bool SaveFile()
        {
            //if (String.IsNullOrEmpty(Destination))
            //    Destination = @"c:\";
            DirectoryInfo directoryinfo = new DirectoryInfo(Path.GetDirectoryName(Destination));
            if (directoryinfo.Exists)
            {
                var content = BuildInformation();
                File.AppendAllText(Destination, content);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Private methods (Stringbuilder)
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
            _ = Content.Append("Bruttozeit\t" + WorkTimeMeasurementModel.GrossWorkTime + " (hh:mm)"+ "\n");
            _ = Content.Append("Timecard\t" + WorkTimeMeasurementModel.Timecard + "\n");
            _ = Content.Append("------------------------------------\n");
            return Content.ToString();
        }
        #endregion
    }

}
