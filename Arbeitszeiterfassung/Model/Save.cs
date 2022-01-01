using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbeitszeiterfassung.Model;
using System.Diagnostics;
using System.IO;
using Arbeitszeiterfassung.Client.Common.Converters;
using System.Threading.Tasks;
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

        static private string _folder;
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
                _workTimeMeasurementModel = worktimemeasurementmodel; 
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => _content = value;
        }

        public void SaveFile()
        {
            BuildInformationString();
            File.WriteAllText(@"C:\Users\Lenovo\Desktop\", "Arbeitszeiterfassung.txt");
        }

        public StringBuilder BuildInformationString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Start Work " + WorkTimeMeasurementModel.StartWork + "\n");
            sb.Append("Start Break " + WorkTimeMeasurementModel.StartBreak + "\n");
            sb.Append("Continue Work " + WorkTimeMeasurementModel.ContinueWork + "\n");
            sb.Append("Finish Work " +  WorkTimeMeasurementModel.FinishWork + "\n");
            sb.Append("NetWorkTime: " +  WorkTimeMeasurementModel.NetWorkTime + "\n");
            sb.Append("GrossWorkTime: " + WorkTimeMeasurementModel.GrossWorkTime + "\n");
            MessageBox.Show(sb.ToString());
            return sb;
        }
    }
}
