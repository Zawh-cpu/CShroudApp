using Avalonia;
using Avalonia.Controls;

namespace CShroudApp.Desktop.Resources.AttachedProperties;

public class IconProperties
{
    public static readonly AttachedProperty<string> IconPathProperty =
        AvaloniaProperty.RegisterAttached<IconProperties, Control, string>("IconPath");

    public static string GetIconPath(AvaloniaObject obj) =>
        obj.GetValue(IconPathProperty);

    public static void SetIconPath(AvaloniaObject obj, string value) =>
        obj.SetValue(IconPathProperty, value);
    
    private IconProperties() { }
}
