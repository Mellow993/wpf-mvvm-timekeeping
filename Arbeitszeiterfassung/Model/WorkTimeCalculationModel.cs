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
        #region Properties Zeitangaben

        private DateTime _arbeitsBeginn;
        public DateTime Arbeitsbeginn
        {
            get => _arbeitsBeginn;
            set
            {
                if (_arbeitsBeginn != value)
                {
                    _arbeitsBeginn = value;
                    this.AddiereArbeitszeit();
                    this.OnPropertyChanged();
                    this.OnPropertyChanged("KurzerTag");
                }
            }
        }

        private DateTime _kuzerTag;
        public DateTime KurzerTag
        {
            get => _kuzerTag;
            private set => _kuzerTag = value;
        }

        private DateTime _normalerTag;
        public DateTime NormalerTag
        {
            get => _normalerTag;
            private set => _normalerTag = value;
        }

        private DateTime _langerTag;
        public DateTime LangerTag
        {
            get => _langerTag;
            private set => _langerTag = value;
        }
        #endregion

        #region Properties Zeitspannen ohne Pause
        private TimeSpan Kurz { get => new TimeSpan(6, 0, 0); }
        private TimeSpan Normal { get => new TimeSpan(7, 36, 0); }
        private TimeSpan Lang { get => new TimeSpan(10, 0, 0); }

        #endregion

        #region Properties Zeitspanne Pausen
        private TimeSpan _zusätzlichePause;
        public TimeSpan ZusätzlichePause
        {
            get => _zusätzlichePause;
            set
            {
                if (_zusätzlichePause != value)
                {
                    _zusätzlichePause = value;

                }
            }
        }
        #endregion

        #region Methoden Berechnung der Arbeitszeit
        private void AddiereArbeitszeit()
        {
            KurzerTag = Arbeitsbeginn.Add(Kurz); //AddHours(6);
            NormalerTag = Arbeitsbeginn.Add(Normal);
            LangerTag = Arbeitsbeginn.Add(Lang);
        }
        #endregion
    }
}
