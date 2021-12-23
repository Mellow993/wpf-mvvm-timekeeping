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
        public WorkTimeCalculationModel Arbeitszeit { get; set; }
        public WorkTimeCalculationViewModel()
        {
            Arbeitszeit = new WorkTimeCalculationModel();
        }
    }
}
