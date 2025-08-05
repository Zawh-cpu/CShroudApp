using System.Globalization;
using Avalonia.Data.Converters;

namespace CShroudApp.Desktop.Resources.Converters;

public class GentleEqualConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        
        return value?.ToString() == parameter?.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not null && value is bool and true)
            return parameter;
        throw new NotSupportedException();
    }
}