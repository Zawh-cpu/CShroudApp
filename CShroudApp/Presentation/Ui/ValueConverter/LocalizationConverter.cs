using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Presentation.Ui.ValueConverter;

public class LocalizationConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (values[0])
        {
            case string city:
            {
                var key = $"{city}";
                return LocalizationService.Translate(key); // или твой способ локализации
            }
            case UnsetValueType:
                return "";
            default:
                return "loc-err";
        }
    }
}