﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbeitszeiterfassung.ViewModel;

namespace Arbeitszeiterfassung.Model
{
    class WorkTimeMeasurementModel : ViewModelBase
    {
        enum State
        {
            VormArbeiten,
            AmArbeiten,
            InPause,
            Fertig
        }
        private DateTime _startWork;
        public DateTime StartWork
        {
            get => _startWork;
            set => _startWork = value;
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
