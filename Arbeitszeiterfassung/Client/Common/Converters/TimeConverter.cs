using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Arbeitszeiterfassung.Client.Common.Converters
{
    public abstract class TimeConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
      
    }
    

    public class ConvertTimeSpan : TimeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter.ToString() == "TimeSpanToString")
            {
                var worktimespan = (TimeSpan)value;
                return worktimespan.ToString(@"hh\:mm");
            }
            else
                return "00:00";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertTimeSpanToMiniutes : TimeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter.ToString() == "TimeSpanMinutesToString")
            {
                var worktimespanminutes = (TimeSpan)value;
                var test = worktimespanminutes.TotalMinutes.ToString("#");
                return test; // worktimespanminutes.TotalMinutes.ToString("#");
            }
            else
                return "0";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertTime : TimeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter.ToString() == "TimeToString")
            {
                DateTime Datum = (DateTime)value;
                return Datum.ToShortTimeString();
            }
            else
                return "07:00";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    public class ConvertMinutes : TimeConverter // : TimeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter.ToString() == "MinutesIntToString")
            {
                int minutes = (int)value;
                return minutes.ToString();
            }
            else
                return "0";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    public class ConvertDate : TimeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter.ToString() == "DateTimeToString")
            {
                DateTime Today = (DateTime)value;
                return Today.ToString("D", CultureInfo.CreateSpecificCulture("de-DE"));
            }
            else
                return value;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConvertStringBuilderToString : TimeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return true;
            }   
            else
            {
                return false;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
