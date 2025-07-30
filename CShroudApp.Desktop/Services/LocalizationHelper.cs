using System.Globalization;
using System.Resources;

namespace CShroudApp.Desktop.Services;

public static class LocalizationHelper
{
    private static readonly ResourceManager ResourceManager = 
        new("CShroudApp.Desktop.Assets.Localization.Resources", typeof(LocalizationHelper).Assembly);

    public static string GetTranslation(string key, CultureInfo culture)
    {
        return ResourceManager.GetString(key, culture) ?? key;
    }
}
