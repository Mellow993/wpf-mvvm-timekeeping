using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbeitszeiterfassung.ViewModel;
using System.Windows;

namespace Arbeitszeiterfassung.Model
{
    class WorkTimeMeasurementModel : ViewModelBase
    {

        #region Fields and properties timespans without break
        private string _state;
        public string State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    UpdateUserinterface();
                }
            }
        }

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
                    SetState("work");
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
            get => _startBreak;
            set
            {
                if(_startBreak != value)
                {
                    _startBreak = value;
                    SetState("break");
                    UpdateUserinterface();
                }
            }
        }

        private TimeSpan _breakTime;
        public TimeSpan BreakTime
        {
            get => _breakTime;
            set => _breakTime = value;
        }

        private DateTime _continueWork;
        public DateTime ContinueWork
        {
            get => _continueWork;
            set
            {
                if (_continueWork != value)
                {
                    _continueWork = value;
                    SetState("work");
                    UpdateUserinterface();
                }
            }
        }

        private DateTime _finishWork;
        public DateTime FinishWork
        {
            get => _finishWork;
            set
            {
                if(_finishWork != value)
                {
                    _finishWork = value;
                    SetState("end");
                    UpdateUserinterface();
                }
            }
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

        private decimal _timecard;
        public decimal Timecard
        {
            get => _timecard;
            set => _timecard = value;
        }
        #endregion

        #region Properties TimeSpans
        private TimeSpan Short { get => new TimeSpan(6, 0, 0); }
        private TimeSpan Normal { get => new TimeSpan(8, 6, 0); }
        private TimeSpan Long { get => new TimeSpan(10, 51, 0); }
        #endregion

        #region Public methods
        public void CalculateTimeSpan()
        {
            if (StartBreak == DateTime.MinValue)
                CalulateTimeSpanWithoutBreak();
            else
                CalculateTimeSpanWithBreak();
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
            Timecard = Convert.ToDecimal(NetWorkTime.TotalHours);
            UpdateUserinterface();
        }

        private void CalculateTimeSpanWithBreak()
        {
            var timeFromStartTillBreak = StartBreak.Subtract(StartWork);
            //BreakTime = ContinueWork.Subtract(StartBreak);
            var timeFromBreakTillFinish = FinishWork.Subtract(ContinueWork);
            BreakTime = new TimeSpan(1, 1, 1); //DEbugging
            NetWorkTime = new TimeSpan(8, 36, 5); // #debugging //timeFromStartTillBreak + timeFromBreakTillFinish;
            GrossWorkTime = new TimeSpan(10, 30, 0); //#debugging // timeFromStartTillBreak + BreakTime + timeFromBreakTillFinish;
            Timecard = Convert.ToDecimal(NetWorkTime.TotalHours);
            UpdateUserinterface();
        }

        private void UpdateUserinterface()
        {
            DisplayLeftSite();
            DisplayRightSite();
        }

        private void DisplayLeftSite()
        {
            OnPropertyChanged(nameof(BreakTime));
            OnPropertyChanged(nameof(StartWork));
            OnPropertyChanged(nameof(ShortDay));
            OnPropertyChanged(nameof(NormalDay));
            OnPropertyChanged(nameof(LongDay));
            OnPropertyChanged(nameof(Timecard));
        }

        private void DisplayRightSite()
        {
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(NetWorkTime));
            OnPropertyChanged(nameof(GrossWorkTime));
        }

        private void SetState(string state)
        {
            switch (state)
            {
                case "work":
                    State = "Am Arbeiten";
                    break;
                case "break":
                    State = "In Pause";
                    break;
                case "end":
                    State = "Feierabend";
                    break;
                default:
                    State =" ";
                    break;
            }
        }
        #endregion
    }
}
