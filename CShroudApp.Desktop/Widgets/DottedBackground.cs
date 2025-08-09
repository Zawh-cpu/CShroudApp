using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace CShroudApp.Desktop.Widgets;

public class DottedBackground : Control
{
    public static readonly StyledProperty<string> DotsColorProperty =
        AvaloniaProperty.Register<DottedBackground, string>(nameof(DotsColor), "#FFFFFF");
    
    public string DotsColor
    {
        get => GetValue(DotsColorProperty);
        set => SetValue(DotsColorProperty, value);
    }
    
    public static readonly StyledProperty<string> BackgroundProperty =
        AvaloniaProperty.Register<DottedBackground, string>(nameof(Background), "#FFFFFF");
    
    public string Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    
    public static readonly StyledProperty<uint> DotRadiusProperty =
        AvaloniaProperty.Register<DottedBackground, uint>(nameof(DotRadius), 2);
    
    public uint DotRadius
    {
        get => GetValue(DotRadiusProperty);
        set => SetValue(DotRadiusProperty, value);
    }
    
    
    public override void Render(DrawingContext context)
    {
        var bounds = Bounds;
        var dotBrush = new SolidColorBrush(Color.Parse(DotsColor));

        for (double y = 0; y < bounds.Height; y += 100)
        {
            for (double x = 0; x < bounds.Width; x += 100)
            {
                var dot1 = new EllipseGeometry(new Rect(x + 25 - DotRadius, y + 25 - DotRadius, DotRadius * 2, DotRadius * 2));
                context.DrawGeometry(dotBrush, null, dot1);
                
                var dot2 = new EllipseGeometry(new Rect(x + 75 - DotRadius, y + 75 - DotRadius, DotRadius * 2, DotRadius * 2));
                context.DrawGeometry(dotBrush, null, dot2);
            }
        }

        base.Render(context);
    }
}