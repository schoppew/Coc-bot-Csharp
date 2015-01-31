namespace CoC.Bot.UI.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    using CoC.Bot.Data;

    public class SelectedTreeViewItemConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TroopModel)
            {
                TroopModel item = (TroopModel)value;

                if (item != null)
                    return item.Name;
            }

            return "Select a Troop";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}