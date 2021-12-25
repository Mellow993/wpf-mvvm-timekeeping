using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    class WorkTimeMeasurementModel
    {
        private DateTime _startWork;
        public DateTime StartWork
        {
            get => _startWork;
            set => _startWork = value;
        }

        private DateTime _startBreak;
        public DateTime StartBreak
        {
            get => _startBreak;
            set => _startBreak = value;
        }
        private DateTime _finishWork;
        public DateTime FinishWork
        {
            get => _finishWork;
            set => _finishWork = value;
        }

        private TimeSpan _entireWorkTime;
        public TimeSpan EntireWorkTime
        {
            get => _entireWorkTime;
            private set => _entireWorkTime = value;
        }

        private void CalculateTimeSpan()
        {

        }
    }
}
