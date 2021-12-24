using Arbeitszeiterfassung.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Client.ViewModel
{
    class WorkTimeCalculationViewModel
    {
        public WorkTimeCalculationModel WorktimeCalculation { get; set; }
        public WorkTimeCalculationViewModel()
        {
            WorktimeCalculation = new WorkTimeCalculationModel();
        }
    }
}
