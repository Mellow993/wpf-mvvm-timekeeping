using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    public class Validation
    {
        private static  DateTime ServiceTimeStart { get => new DateTime(DateTime.Now.Year , DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0); }
        private static DateTime ServiceTimeEnd { get => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0); }
         
        public static bool IsServiceTime(DateTime starttime, DateTime endtime) 
        { 
            if(starttime <= ServiceTimeStart || endtime >= ServiceTimeEnd) 
                return false;
            
            else
                return true; 
        }

        public bool IsLegalInput() { return true; }
    }
}
