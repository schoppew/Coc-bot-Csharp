namespace CoC.Bot.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Controls;

    /// <summary>
    /// A Left Margin Multiplier Converter.
    /// </summary>
    public class LeftMarginMultiplierConverter : IValueConverter
    {
        public double Length { get; set; }

        static int GetDepth(TreeViewItem item)
        {
            FrameworkElement elem = item;
            while (VisualTreeHelper.GetParent(elem) != null)
            {
                var tvi = VisualTreeHelper.GetParent(elem) as TreeViewItem;
                if (tvi != null)
                    return GetDepth(tvi) + 1;
                elem = VisualTreeHelper.GetParent(elem) as FrameworkElement;
            }
            return 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as TreeViewItem;
            if (item == null)
                return new Thickness(0);

            return new Thickness(Length * GetDepth(item), 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}