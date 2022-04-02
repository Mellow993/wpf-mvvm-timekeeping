using System.Text;
using System.IO;

namespace Arbeitszeiterfassung.Model
{
    class Save
    {
        private string Destination { get; set; }
        public WorkTimeMeasurementModel WorkTimeMeasurementModel { get; private set; }
        public StringBuilder Content { get; private set; }

        public Save(WorkTimeMeasurementModel worktimemeasurementmodel, string destination)
        {
            WorkTimeMeasurementModel = worktimemeasurementmodel;
            Destination = destination;
        }

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

        private void PrepareOutput() => File.AppendAllText(Destination, BuildInformation());
        private string BuildInformation()
        {
            StringBuilder Content = new();
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
