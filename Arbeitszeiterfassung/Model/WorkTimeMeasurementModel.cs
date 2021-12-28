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

        #region Properties timespans without break
        public string[] State = new string[] { "", "Arbeiten", "Pause", "Feierabend"};

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

        #region Properties TimeSpans

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
                    UpdateUserinterface();
                }
            }
        }

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
        #endregion

        #region Public methods
        public void CalculateTimeSpan()
        {
            if (Validation.IsServiceTime(StartWork, FinishWork))
            {
                if (StartBreak == DateTime.MinValue)
                    CalulateTimeSpanWithoutBreak();
                else
                    CalculateTimeSpanWithBreak();
            }
            else
                throw new Exception("wrong service time");

        }
        #endregion

        #region Private methods
        private void CalculateWorkTime()
        {
            ShortDay = StartWork.Add(Short); //.AddMinutes(BreakTimeInMinutes);
            NormalDay = StartWork.Add(Normal); //.AddMinutes(BreakTimeInMinutes);
            LongDay = StartWork.Add(Long); //.AddMinutes(BreakTimeInMinutes);
            UpdateUserinterface();
        }

        private void CalulateTimeSpanWithoutBreak()
        {
            NetWorkTime= FinishWork.Subtract(StartWork);
            GrossWorkTime = NetWorkTime;
            UpdateUserinterface();
        }

        private void CalculateTimeSpanWithBreak()
        {
            var timeFromStartTillBreak = StartBreak.Subtract(StartWork);
            //BreakTime = ContinueWork.Subtract(StartBreak);
            var timeFromBreakTillFinish = FinishWork.Subtract(ContinueWork);
            BreakTime = new TimeSpan(1, 1, 1); //DEbugging
            NetWorkTime = new TimeSpan(8, 36, 5); // #debugging //timeFromStartTillBreak + timeFromBreakTillFinish;
            GrossWorkTime = new TimeSpan(10, 58, 0); //#debugging // timeFromStartTillBreak + BreakTime + timeFromBreakTillFinish;
            UpdateUserinterface();
        }

        private void UpdateUserinterface()
        {
            OnPropertyChanged(nameof(StartWork));
            OnPropertyChanged(nameof(ShortDay));
            OnPropertyChanged(nameof(NormalDay));
            OnPropertyChanged(nameof(LongDay));
            OnPropertyChanged(nameof(BreakTime));
            OnPropertyChanged(nameof(NetWorkTime));
            OnPropertyChanged(nameof(GrossWorkTime));
        }
        #endregion
    }
}
