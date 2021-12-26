using Arbeitszeiterfassung.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    class WorkTimeCalculationModel : ViewModelBase
    {
        #region Properties diffrent times
        public DateTime Today { get => DateTime.Now; }

        private DateTime _startOfWork;
        public DateTime StartOfWork
        {
            get => _startOfWork;
            set
            {
                if (_startOfWork != value)
                {
                    _startOfWork = value;
                    CalculateWorkTime();
                    RefreshUserInterface();
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
        #endregion

        #region Properties timespans without break
        private TimeSpan Short { get => new TimeSpan(6, 0, 0); }
        private TimeSpan Normal { get => new TimeSpan(8, 6, 0); }
        private TimeSpan Long { get => new TimeSpan(10, 51, 0); }
        #endregion

        #region Properties additional break
        private int _breakTimeInMinutes;
        public int BreakTimeInMinutes
        {
            get => _breakTimeInMinutes;
            set
            {
                if(_breakTimeInMinutes != value)
                {
                    _breakTimeInMinutes = value;
                    CalculateWorkTime();
                    RefreshUserInterface();
                }
            }
        }
        #endregion

        #region Method calculate daily work time
        private void CalculateWorkTime()
        {
            ShortDay = StartOfWork.Add(Short).AddMinutes(BreakTimeInMinutes);
            NormalDay = StartOfWork.Add(Normal).AddMinutes(BreakTimeInMinutes);
            LongDay = StartOfWork.Add(Long).AddMinutes(BreakTimeInMinutes);
        }

        private void RefreshUserInterface()
        {
            OnPropertyChanged();
            OnPropertyChanged("ShortDay");
            OnPropertyChanged("NormalDay");
            OnPropertyChanged("LongDay");
        }
        #endregion
    }
}
