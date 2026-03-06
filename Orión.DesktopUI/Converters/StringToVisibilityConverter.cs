using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Orión.DesktopUI.Converters;

public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return Visibility.Collapsed;
        
        string val = value.ToString();
        string param = parameter.ToString();
        
        return val.Equals(param, StringComparison.OrdinalIgnoreCase) 
            ? Visibility.Visible 
            : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
