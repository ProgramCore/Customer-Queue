using CustomerQueue.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CustomerQueue.Converters
{
    public class TimeUrgencyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                int minutes = Helpers.MinuteDifference(DateTime.Now, (DateTime)value);

                if (minutes <= 5)
                {
                    return Color.FromHex("#0f9246");
                }
                else if (minutes <= 10)
                {
                    return Color.FromHex("#7dbb42");
                }
                else if (minutes <= 20)
                {
                    return Color.FromHex("#f68e1f");
                }
                else if (minutes <= 25)
                {
                    return Color.FromHex("#ef4723");
                }
                else
                {
                    return Color.FromHex("#bc2026");
                }

            }

            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }

    public class AlertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var resv = (DateTime)value;

                return resv.Ticks > DateTime.MinValue.Ticks;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    public class BoolNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    public class WaitMinsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                return Helpers.MinuteDifference(DateTime.Now, (DateTime)value);
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    public class AlertedMinsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int mins = 0;

            if (value is DateTime)
            {
                var time = (DateTime)value;

                if(time.Ticks > DateTime.MinValue.Ticks)
                {
                    mins = Helpers.MinuteDifference(DateTime.Now, time);
                }
                
            }

            return mins;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
