using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CShroudApp.Desktop.Resources.Converters;

public class ColorToCssConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string? str;
        
        switch (value)
        {
            case IBrush brush:
            {
                Console.WriteLine("ITS A BRUSH!!!!");
                if (brush is ISolidColorBrush solid)
                {
                    str = solid.Color.ToString();
                    return $"* {{ fill: #{(str.Length >= 8 ? str[3..] : str)}; }}";
                }
            
                str = value.ToString();
                return $"* {{ fill: {(str != null && str.Length >= 8 ? str[2..] : str)}; }}";
            }
            case Color color:
                return $"* {{ fill: #{color.R:X2}{color.G:X2}{color.B:X2}; }}";
        }
        
        return "* {{ fill: black; }}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}