using Avalonia;
using Avalonia.Controls;

namespace CShroudApp.Desktop.Widgets;

public class VariableGrid : Panel
{
    public static readonly StyledProperty<double> MaxColumnsProperty =
        AvaloniaProperty.Register<DottedBackground, double>(nameof(MaxColumns), double.MaxValue);
    
    public double MaxColumns
    {
        get => GetValue(MaxColumnsProperty);
        set => SetValue(MaxColumnsProperty, value);
    }
    
    protected override Size MeasureOverride(Size availableSize)
    {
        var maxWidth = 0.0;
        var maxHeight = 0.0;
        foreach (var child in Children)
        {
            child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (child.DesiredSize.Width > maxWidth)
                maxWidth = child.DesiredSize.Width;
            if (child.DesiredSize.Height > maxHeight)
                maxHeight = child.DesiredSize.Height;
        }
        
        var columnCount = Math.Min( availableSize.Width / maxWidth < 1 ? 1 : Math.Floor(availableSize.Width / maxWidth), MaxColumns );
        return new Size(availableSize.Width, maxHeight * Math.Ceiling(Children.Count / columnCount));
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (!Children.Any()) return finalSize;
        
        var maxMinChildWidth = Children.Max(c => c.DesiredSize.Width);
        
        var columnCount = Math.Min( finalSize.Width / maxMinChildWidth < 1 ? 1 : Math.Floor(finalSize.Width / maxMinChildWidth), MaxColumns );
        var colWidth = finalSize.Width / columnCount;
        
        var rowSize = Children.Max(c => c.DesiredSize.Height);
        
        var i = 0;
        double offsetY = 0;
        double offsetX = 0;
        foreach (var child in Children)
        {
            child.Arrange(new Rect(offsetX, offsetY, colWidth, rowSize));
            i++;
            offsetX += colWidth;

            if (i >= columnCount)
            {
                i = 0;
                offsetY += rowSize;
                offsetX = 0;
            }
        }

        return finalSize;
    }
}
