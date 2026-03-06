using System;
using System.Globalization;
using System.Windows.Data;

namespace Orión.DesktopUI.Converters;

public class StringToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null) return false;
        return value.ToString()?.Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && (bool)value && parameter != null) return parameter.ToString() ?? string.Empty;
        return Binding.DoNothing;
    }
}
