using System;
using Microsoft.Win32;

namespace Arbeitszeiterfassung.Model
{
    class UserSettings
    {
        #region Fields and properties
        private const string _pathToKey = @"HKEY_CURRENT_USER\SOFTWARE\Arbeitszeiterfassung";
        private const string _keyValue = "Pfad";
        public string SavePath { get; private set; }
        #endregion


        #region Constructors
        public UserSettings() { }
        public UserSettings(string savepath) { SavePath = savepath; }
        #endregion


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
        private static bool TheKeyExits()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_pathToKey, true);
            if (key == null)
                return false;
            else
                return true;
        }
        private static void CreateSubKey() => Registry.CurrentUser.CreateSubKey(_pathToKey, true);
    }
}

