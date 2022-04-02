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

        private int _breakTimeInMinutes;
        private string _state;
        private DateTime _startWork;
        private DateTime _shortDay;
        private DateTime _finishWork;
        private DateTime _continueWork;
        public int BreakTimeInMinutes
        {
            get => _breakTimeInMinutes;
            set
            {
                if (_breakTimeInMinutes != value)
                {
                    _breakTimeInMinutes = value;
                    UpdateUserinterface();
                }
            }
        }
        public bool InServiceTime { get ; private set; }
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
                    CheckServiceTime();
                    UpdateUserinterface();
                }
            }
        }
        public DateTime ShortDay
        {
            get => _shortDay;
            private set => _shortDay = value;
        }
        public DateTime NormalDay { get; private set; }
        public DateTime LongDay { get; private set; }
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
        public TimeSpan BreakTime { get; private set; }
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
        public TimeSpan NetWorkTime { get; set; }
        public TimeSpan GrossWorkTime { get; set; }
        public decimal Timecard { get; set; }
        private TimeSpan Short { get => new TimeSpan(6, 0, 0); }
        private TimeSpan Normal { get => new TimeSpan(8, 6, 0); }
        private TimeSpan Long { get => new TimeSpan(10, 51, 0); }

        public void CalculateTimeSpan()
        {
            if (StartBreak == DateTime.MinValue)
                CalulateTimeSpanWithoutBreak();
            else
                CalculateTimeSpanWithBreak();
        }
        

        #region Private methods
        private void CalculateWorkTime()
        {
            ShortDay = StartWork.Add(Short);
            NormalDay = StartWork.Add(Normal);
            LongDay = StartWork.Add(Long); 
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
            var timeFromBreakTillFinish = FinishWork.Subtract(ContinueWork);
            BreakTime = ContinueWork.Subtract(StartBreak);
            NetWorkTime = timeFromStartTillBreak + timeFromBreakTillFinish;
            GrossWorkTime = timeFromStartTillBreak + BreakTime + timeFromBreakTillFinish;
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

        private void CheckServiceTime()
        {
            if (ValidateServiceTime())
                InServiceTime = true; 
            else
                InServiceTime = false;
        }
        
        private bool ValidateServiceTime() => Validation.IsServiceTime(StartWork, LongDay);
        #endregion
    }
}
