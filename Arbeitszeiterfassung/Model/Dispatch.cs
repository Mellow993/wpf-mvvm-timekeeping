using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    internal class Dispatch : EventArgs
    {
        private string _message = string.Empty;
        public string Message { get => _message; set => _message = value; }

        Dispatch(string message) { _message = message; }
    }
}
