namespace CoC.Bot.Chart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Data;

    public class DoubleToGridLengthPercent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return InternalConvert(value, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object InternalConvert(object value, Type targetType, object parameter)
        {
            double percentage = (double)value;
            if (parameter != null)
            {
                if (percentage <= 1)
                {
                    return new GridLength(1.0 - (double)percentage, GridUnitType.Star);
                }
                return new GridLength(100.0 - (double)percentage, GridUnitType.Star);
            }
            else
            {
                return new GridLength((double)percentage, GridUnitType.Star);
            }
        }
    }
}
