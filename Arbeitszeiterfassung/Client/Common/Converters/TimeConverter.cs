using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arbeitszeiterfassung.Client.Common.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter.ToString() == "TimeToString")
            {
                DateTime Datum = (DateTime)value;
                return Datum.ToShortTimeString();
            }
            else
                return "07:00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime arbeitszeit;
                DateTime.TryParse(value.ToString(), out arbeitszeit);
                return arbeitszeit;
            }
            else
                return "07:00";
        }
    }
}
