using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AdLib.UI.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        // Convert from int to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                // If intValue is greater than 0, return Visible, otherwise Collapsed
                return intValue > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;  // Default value if not an int
        }

        // ConvertBack is not usually needed for one-way bindings
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
