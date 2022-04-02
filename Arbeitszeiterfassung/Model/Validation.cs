using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    internal class Validation
    {
        private static  DateTime ServiceTimeStart { get => new DateTime(DateTime.Now.Year , DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0); }
        private static DateTime ServiceTimeEnd { get => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0); }
        internal static bool IsServiceTime(DateTime starttime, DateTime endtime)
            => starttime <= ServiceTimeStart || endtime >= ServiceTimeEnd ? false : true;
    }
}
