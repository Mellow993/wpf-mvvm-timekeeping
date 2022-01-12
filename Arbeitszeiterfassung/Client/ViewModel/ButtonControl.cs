using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Client.ViewModel
{
    class ButtonControl
    {
        public enum State
        {
            None,
            Work,
            Break,
            ContinueWork,
            HomeTime
        }

        public State _currentState;
        public State CurrentState
        {
            get => _currentState;
            set => _currentState = value; // on property changed?
        }
    }
}
