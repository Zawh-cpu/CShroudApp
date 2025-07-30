using System.Globalization;
using System.Resources;
using Avalonia;
using Avalonia.Data.Converters;
using CShroudApp.Desktop.Services;

namespace CShroudApp.Desktop.Resources.Converters;

public class ResourceKeyToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string key)
            return LocalizationHelper.GetTranslation(key, CultureInfo.CurrentCulture);
        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) 
        => throw new NotImplementedException();
}
