using System.Globalization;
using Avalonia.Markup.Xaml;
using CShroudApp.Desktop.Services;

namespace CShroudApp.Desktop.Resources.MarkupExtensions;

public class LocalizationExtension : MarkupExtension
{
    public string Key { get; set; }

    public LocalizationExtension(string key)
    {
        Key = key;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return LocalizationHelper.GetTranslation(Key, CultureInfo.CurrentCulture);
    }
}