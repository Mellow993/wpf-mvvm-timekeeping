using System;
using Microsoft.Win32;

namespace Arbeitszeiterfassung.Model
{
    class UserSettings
    {
        private const string _pathToKey = @"HKEY_CURRENT_USER\SOFTWARE\Arbeitszeiterfassung";
        private const string _keyValue = "Pfad";

        public event EventHandler PathHasChanged;

        private string _savePath;
        public string SavePath
        {
            get => _savePath;
            private set => _savePath = value;
        }

        public UserSettings() { }
        public UserSettings(string savePath) { _savePath = savePath; }

        public void SetRegistry() => Registry.SetValue(_pathToKey, _keyValue, SavePath);
    
        public string ReadRegistry()
        {
            if (TheKeyExits())
                return (string)Registry.GetValue(_pathToKey, _keyValue, null);

            else
            {
                CreateSubKey();
                return String.Empty;
            }
        }

        private static void CreateSubKey() => Registry.CurrentUser.CreateSubKey(_pathToKey, true);

        private static bool TheKeyExits()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_pathToKey, true);
            if (key == null)
                return false;
            else
                return true;
        }
    }
}

