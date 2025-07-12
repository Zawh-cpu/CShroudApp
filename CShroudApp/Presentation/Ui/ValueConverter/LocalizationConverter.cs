using System.Globalization;
using Avalonia.Data.Converters;
using CShroudApp.Infrastructure.StaticServices;

namespace CShroudApp.Presentation.Ui.ValueConverter;

public class LocalizationConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is string city)
        {
            var key = $"{city}";
            return LocalizationService.Translate(key); // или твой способ локализации
        }
        return "loc-err";
    }
}