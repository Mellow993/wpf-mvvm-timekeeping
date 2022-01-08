using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeiterfassung.Model
{
    public class Validation
    {
        #region Set service times

        private static  DateTime ServiceTimeStart { get => new DateTime(DateTime.Now.Year , DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0); }
        private static DateTime ServiceTimeEnd { get => new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0); }
        #endregion

        #region Compare Inputs
        public static bool IsServiceTime(DateTime starttime, DateTime endtime)
            => (starttime <= ServiceTimeStart || endtime >= ServiceTimeEnd) ? false : true;

        public bool IsLegalInput => true;
        #endregion

    }
}
