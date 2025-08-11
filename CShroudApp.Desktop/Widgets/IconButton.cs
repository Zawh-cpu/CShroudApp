using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CShroudApp.Desktop.Widgets;

public class IconButton : Button
{
    public static readonly StyledProperty<string?> IconPathProperty =
        AvaloniaProperty.Register<IconButton, string?>(nameof(IconPath));

    public string? IconPath
    {
        get => GetValue(IconPathProperty);
        set => SetValue(IconPathProperty, value);
    }
    
    public static readonly StyledProperty<Color> IconColorProperty =
        AvaloniaProperty.Register<IconButton, Color>(
            nameof(IconColor),
            Colors.Black);

    public Color IconColor
    {
        get => GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
    
    public static readonly StyledProperty<uint> IconWidthProperty =
        AvaloniaProperty.Register<IconButton, uint>(nameof(IconWidth), 14);

    public uint IconWidth
    {
        get => GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }
    
    public static readonly StyledProperty<uint> IconHeightProperty =
        AvaloniaProperty.Register<IconButton, uint>(nameof(IconHeight), 14);

    public uint IconHeight
    {
        get => GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }
    
    public static readonly StyledProperty<float> GapProperty =
        AvaloniaProperty.Register<IconButton, float>(nameof(Gap), 7);

    public float Gap
    {
        get => GetValue(GapProperty);
        set => SetValue(GapProperty, value);
    }
    
    public static readonly StyledProperty<float> LineHeightProperty =
        AvaloniaProperty.Register<IconButton, float>(nameof(LineHeight), 21);

    public float LineHeight
    {
        get => GetValue(LineHeightProperty);
        set => SetValue(LineHeightProperty, value);
    }

    static IconButton()
    {
        AffectsRender<IconButton>(IconPathProperty);
    }
}