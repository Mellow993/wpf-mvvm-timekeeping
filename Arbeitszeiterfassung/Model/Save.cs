using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    class Save
    {
        private string _destination;
        public string Destination
        {
            get => _destination;
            set => _destination = value;
        }

        private bool _canWrite;
        public bool CanWrite
        {
            get => _canWrite;
            set => _canWrite = value;
        }

        private bool _folderExits;
        public bool FolderExits
        {
            get => _folderExits;
            set => _folderExits = value;
        }

    }
}
