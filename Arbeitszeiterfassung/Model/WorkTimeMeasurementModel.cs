using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbeitszeiterfassung.ViewModel;

namespace Arbeitszeiterfassung.Model
{
    class WorkTimeMeasurementModel : ViewModelBase
    {
        public enum teststate
        {
            VormArbeiten,
            AmArbeiten,
            InPause,
            Fertig
        }

        public List<string> State = new List<string>()
        {
            "am Arbeiten",
            "in Pause",
            "Fertig"
        };


        public DateTime Today { get => DateTime.Now; }

        private DateTime _startWork;
        public DateTime StartWork
        {
            get => _startWork;
            set
            {
                if (_startWork != value)
                {
                    _startWork = value;
                    CalculateWorkTime();
                    UpdateUserinterface();
                }
            }
        }

        #region Merged from calculation class
        private DateTime _shortDay;
        public DateTime ShortDay
        {
            get => _shortDay;
            private set => _shortDay = value;
        }

        private DateTime _normalDay;
        public DateTime NormalDay
        {
            get => _normalDay;
            private set => _normalDay = value;
        }

        private DateTime _longDay;
        public DateTime LongDay
        {
            get => _longDay;
            private set => _longDay = value;
        }

        #region Properties timespans without break
        private TimeSpan Short { get => new TimeSpan(6, 0, 0); }
        private TimeSpan Normal { get => new TimeSpan(8, 6, 0); }
        private TimeSpan Long { get => new TimeSpan(10, 51, 0); }
        #endregion

        private int _breakTimeInMinutes;
        public int BreakTimeInMinutes
        {
            get => _breakTimeInMinutes;
            set
            {
                if (_breakTimeInMinutes != value)
                {
                    _breakTimeInMinutes = value;
                    //CalculateWorkTime();
                    //UpdateUserinterface();
                }
            }
        }
        #endregion

        private DateTime _startBreak;
        public DateTime StartBreak
        {
            private get => _startBreak;
            set => _startBreak = value;
        }

        private DateTime _continueWork;

        private TimeSpan _breakTime;
        public TimeSpan BreakTime
        {
            get => _breakTime;
            set => _breakTime = value;
        }
        public DateTime ContinueWork
        {
            private get => _continueWork;
            set => _continueWork = value;
        }

        private DateTime _finishWork;
        public DateTime FinishWork
        {
            private get => _finishWork;
            set => _finishWork = value;
        }

        private TimeSpan _neteWorkTime;
        public TimeSpan NetWorkTime
        {
            get => _neteWorkTime;
            set => _neteWorkTime = value;
        }

        private TimeSpan _grossWorkTime;
        public TimeSpan GrossWorkTime
        {
            get => _grossWorkTime;
            set => _grossWorkTime = value;
        }

        private void CalculateWorkTime()
        {
            ShortDay = StartWork.Add(Short); //.AddMinutes(BreakTimeInMinutes);
            NormalDay = StartWork.Add(Normal); //.AddMinutes(BreakTimeInMinutes);
            LongDay = StartWork.Add(Long); //.AddMinutes(BreakTimeInMinutes);
        }

        private void UpdateUserinterface()
        {
            OnPropertyChanged(nameof(StartWork));
            OnPropertyChanged(nameof(ShortDay));
            OnPropertyChanged(nameof(NormalDay));
            OnPropertyChanged(nameof(LongDay));
        }


        public void CalculateTimeSpan()
        {
            if(StartBreak == DateTime.MinValue)
            {
                GrossWorkTime = FinishWork.Subtract(StartWork);
            }
            else
            {
                var timeFromStartTillBreak = StartBreak.Subtract(StartWork);
                BreakTime = ContinueWork.Subtract(StartBreak);
                var timeFromBreakTillFinish = FinishWork.Subtract(ContinueWork);
                NetWorkTime = timeFromStartTillBreak + timeFromBreakTillFinish;
                GrossWorkTime = timeFromStartTillBreak + BreakTime + timeFromBreakTillFinish;
            }
        }
    }
}
