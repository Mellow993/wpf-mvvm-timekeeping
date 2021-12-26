﻿using System;
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


        private TimeSpan _entireWorkTime;
        public TimeSpan EntireWorkTime
        {
            get => _entireWorkTime;
            set => _entireWorkTime = value;
        }


        public void CalculateTimeSpan()
        {
            if(StartBreak == DateTime.MinValue)
            {
                EntireWorkTime = FinishWork.Subtract(StartWork);
            }
            else
            {
                var timeFromStartTillBreak = (TimeSpan)StartBreak.Subtract(StartWork);
                BreakTime = ContinueWork.Subtract(StartBreak);
                var timeFromBreakTillFinish = FinishWork.Subtract(ContinueWork);
                EntireWorkTime = timeFromStartTillBreak + BreakTime + timeFromBreakTillFinish;
            }
        }
    }
}
