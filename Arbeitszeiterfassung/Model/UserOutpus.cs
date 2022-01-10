using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arbeitszeiterfassung.Model
{
    public class UserOutpus
    {
        private object _notifyIcon;

        public event EventHandler FileHasBeenSaved;


        //_notifyIcon.Icon = new System.Drawing.Icon(@"C:\Users\Lenovo\source\repos\Arbeitszeiterfassung\Arbeitszeiterfassung\Client\Icon\icon.ico");
        //_notifyIcon.Visible = true;
        //_notifyIcon.Text = "Arbeitszeiterfassung";

        public void OnSaveCompleted(object sender, EventArgs e)
        {
            FileHasBeenSaved ?.Invoke(this, e);
        }

    }

}
