using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace CShroudApp.Desktop.Resources.Converters;

public class EqualTypeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Type typeValue && parameter is Type typeParameter)
        {
            Console.WriteLine(typeValue);
            Console.WriteLine(typeParameter);
            return typeValue == typeParameter;
        }

        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b && parameter is Type typeParameter)
            return typeParameter;
        
        throw new NotSupportedException();
    }
}