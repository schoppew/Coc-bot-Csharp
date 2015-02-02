namespace CoC.Bot.UI.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Data;

    using System.Globalization;

    /// <summary>
    /// An String to Integer Converter.
    /// </summary>
    public class IntegerToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the value to Integer.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (int)value;
            //return val == 0 ? string.Empty : value.ToString();

            return val.ToString();
        }

        /// <summary>
        /// Converts back the value to string.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int retVal;
            return int.TryParse(value.ToString(), out retVal) ? retVal : 0;
        }
    }
}
