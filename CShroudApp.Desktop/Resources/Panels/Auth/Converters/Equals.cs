using System.Globalization;
using Avalonia.Data.Converters;

namespace CShroudApp.Desktop.Resources.Panels.Auth.Converters;

public class EqualsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum && parameter is Enum)
            return Equals(value, parameter);
        
        return value?.ToString()?.Equals(parameter?.ToString(), StringComparison.Ordinal) == true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}