using System;
using Microsoft.Win32;

namespace Arbeitszeiterfassung.Model
{
    class UserSettings
    {
        private const string _pathToKey = @"HKEY_CURRENT_USER\SOFTWARE\Arbeitszeiterfassung";
        private const string _keyValue = "Pfad";
        public string SavePath { get; private set; }

        internal UserSettings() { }
        internal UserSettings(string savepath) { SavePath = savepath; }

        internal void SetRegistry() => Registry.SetValue(_pathToKey, _keyValue, SavePath);
        internal string ReadRegistry()
        {
            if (KeyExits())
                return (string)Registry.GetValue(_pathToKey, _keyValue, null);

            else
            {
                CreateSubKey();
                return String.Empty;
            }
        }
        private static bool KeyExits()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_pathToKey, true);
            return (key != null) ? true : false;
        }
        private static void CreateSubKey() => Registry.CurrentUser.CreateSubKey(_pathToKey, true);
    }
}

