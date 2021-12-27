using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    public class Validation
    {
        private static  DateTime ServiceTimeStart { get => new DateTime(1, 1, 1, 7, 0, 0); }
        private static DateTime ServiceTimeEnd { get => new DateTime(1, 1, 1, 20, 0, 0); }
         
        public static bool IsServiceTime(DateTime starttime, DateTime endtime) 
        { 
            if(starttime.TimeOfDay <= ServiceTimeStart.TimeOfDay || endtime.TimeOfDay >= ServiceTimeEnd.TimeOfDay)
                return false;
            
            else
                return true; 
        }

        public bool IsLegalInput() { return true; }
    }
}
