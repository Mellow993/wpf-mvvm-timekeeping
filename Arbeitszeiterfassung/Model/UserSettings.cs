using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Arbeitszeiterfassung.Model
{
    class UserSettings
    {
        //RegistryKey key = Registry.CurrentUser.CreateSubKey("AppEvents", true);
        //var keyvalue = key.GetValue("Standard").ToString();
        //MessageBox.Show(keyvalue.ToString());

        private string _savePath;
        public string SavePath
        {
            get => _savePath;
            set => _savePath = value;
        }

        // Keep the class clean, no validation, the validation is made before the constructor is invoked.
        public UserSettings(string savePath) { _savePath = savePath; }

        public bool CheckRegistry()
        {
            //if (exits)
            //    return true;
            //else
            //    return false;
            return true;
        }

        public void SetRegistry(string destination) =>  Registry.SetValue(@"HKEY_CURRENT_USER\Test\Arbeitszeiterfassung", "Pfad", SavePath);

        public string ReadRegistry()
        {
            SavePath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Test\Arbeitszeiterfassung", "Pfad", null);
            return SavePath;
        }

        //RegistryKey keyy = Registry.CurrentUser.OpenSubKey(@"HKEY_CURRENT_USER\Test\Arbeitszeiterfassung", true);
        //keyy = keyy.CreateSubKey("Arbeitszeiterfassung");
        //keyy.SetValue("Pfad", 1, RegistryValueKind.DWord);



    }


}

