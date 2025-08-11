using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CShroudApp.Desktop.Resources.Converters;

public class ColorToBrushConverter : IValueConverter
{
    public static readonly ColorToBrushConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
            return new SolidColorBrush(color);

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ISolidColorBrush brush)
            return brush.Color;

        return AvaloniaProperty.UnsetValue;
    }
}