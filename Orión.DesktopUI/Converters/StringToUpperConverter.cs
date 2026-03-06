using System;
using System.Globalization;
using System.Windows.Data;

namespace Orión.DesktopUI.Converters;

public class StringToUpperConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString()?.ToUpper() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
