using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CShroudApp.Desktop.Resources.AttachedProperties;

public static class ColorProperties
{
    public static readonly AttachedProperty<Color> MainColorProperty =
        AvaloniaProperty.RegisterAttached<Control, Color>("MainColor", typeof(ColorProperties));

    public static readonly AttachedProperty<Color> SecondaryColorProperty =
        AvaloniaProperty.RegisterAttached<Control, Color>("SecondaryColor", typeof(ColorProperties));

    public static void SetMainColor(AvaloniaObject element, Color value) =>
        element.SetValue(MainColorProperty, value);

    public static Color GetMainColor(AvaloniaObject element) =>
        element.GetValue(MainColorProperty);

    public static void SetSecondaryColor(AvaloniaObject element, Color value) =>
        element.SetValue(SecondaryColorProperty, value);

    public static Color GetSecondaryColor(AvaloniaObject element) =>
        element.GetValue(SecondaryColorProperty);
}