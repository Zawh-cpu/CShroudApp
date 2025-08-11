using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using CShroudApp.Desktop.Services;

namespace CShroudApp.Desktop.Resources.Converters;

public class LocalizationConverter : IValueConverter, IMultiValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (value)
        {
            case string key:
            {
                return LocalizationHelper.GetTranslation(key, CultureInfo.CurrentCulture);
            }
            case UnsetValueType:
                return "";
            default:
                return "loc-err";
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (values[0])
        {
            case string key:
            {
                return LocalizationHelper.GetTranslation(key, CultureInfo.CurrentCulture);
            }
            case UnsetValueType:
                return "";
            default:
                return "loc-err";
        }
    }
    
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}