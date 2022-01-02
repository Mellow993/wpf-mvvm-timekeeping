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


namespace Arbeitszeiterfassung.Model
{
    class Save
    {


        private WorkTimeMeasurementModel _workTimeMeasurementModel;
        public WorkTimeMeasurementModel WorkTimeMeasurementModel
        {
            get => _workTimeMeasurementModel;
            set => _workTimeMeasurementModel = value;
        }

        private string _folder;
        public string Folder
        {
            get => _folder;
            set => _folder = value;
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }
        public string Destination
        {
            get => Folder + FileName;
        }
        public Save(WorkTimeMeasurementModel worktimemeasurementmodel) 
        { 
            if(worktimemeasurementmodel != null)
            {
                _workTimeMeasurementModel = worktimemeasurementmodel;
                _folder = @"C:\Users\Lenovo\Desktop\";
                _fileName = "Arbeitszeiterfassung.txt";
                // @"C:\Users\Lenovo\Desktop\Arbeitszeiterfassung.txt";
            }
        }

        private StringBuilder _content;
        public StringBuilder Content
        {
            get => _content;
            set => _content = value;
        }

        public bool SaveFile()
        {
            DirectoryInfo di = new DirectoryInfo(Folder);
            if (di.Exists)
            {
                string dailycontent = BuildInformation();
                File.AppendAllText(Destination, dailycontent); // Content.ToString()); // Content.ToString());
                return true;
            }
            else
                return false;
        }

        public string BuildInformation()
        {
            StringBuilder Content = new StringBuilder();
            _ = Content.Append("\nStart Work " + WorkTimeMeasurementModel.StartWork + "\n");
            _ = Content.Append("Start Break " + WorkTimeMeasurementModel.StartBreak + "\n");
            _ = Content.Append("Continue Work " + WorkTimeMeasurementModel.ContinueWork + "\n");
            _ = Content.Append("Finish Work " + WorkTimeMeasurementModel.FinishWork + "\n");
            _ = Content.Append("NetWorkTime: " + WorkTimeMeasurementModel.NetWorkTime + "\n");
            _ = Content.Append("GrossWorkTime: " + WorkTimeMeasurementModel.GrossWorkTime + "\n");
            _ = Content.Append("--------------------------------\n");
            return Content.ToString();
        }
    }
}
