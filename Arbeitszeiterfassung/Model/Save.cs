using System.Text;
using System.IO;

namespace Arbeitszeiterfassung.Model
{
    class Save
    {
        #region Attributes
        public WorkTimeMeasurementModel WorkTimeMeasurementModel { get; set; }
        private string Destination { get; set; }
        public StringBuilder Content { get; set; }

        #endregion

        #region Constructor
        public Save(WorkTimeMeasurementModel worktimemeasurementmodel, string destination)
        {
            WorkTimeMeasurementModel = worktimemeasurementmodel;
            Destination = destination;
        }
        #endregion

        #region Public methods (SaveFile)
        public bool SaveFile()
        {
            DirectoryInfo directoryinfo = new DirectoryInfo(Path.GetDirectoryName(Destination));
            if (directoryinfo.Exists)
            {
                PrepareOutput();
                return true;
            }
            else
                return false;
        }
        #endregion

        #region Private methods (Stringbuilder)
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
        #endregion
    }
}
