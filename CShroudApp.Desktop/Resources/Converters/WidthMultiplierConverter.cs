using System.Globalization;
using Avalonia.Data.Converters;

namespace CShroudApp.Desktop.Resources.Converters;

public class WidthMultiplierConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width && double.TryParse(parameter?.ToString(), out var factor))
        {
            return width * factor;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
