namespace CShroudApp.Desktop.Widgets;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using System;
using System.Linq;

public class AdaptiveGridPanel : Panel
{
    public static readonly AttachedProperty<double> PriorityWeightProperty =
        AvaloniaProperty.RegisterAttached<AdaptiveGridPanel, Control, double>("PriorityWeight", 1);

    public static double GetPriorityWeight(Control control) => control.GetValue(PriorityWeightProperty);
    public static void SetPriorityWeight(Control control, double value) => control.SetValue(PriorityWeightProperty, value);

    protected override Size MeasureOverride(Size availableSize)
    {
        double totalMinWidth = 0;
        double maxHeight = 0;
        
        foreach (var child in Children)
        {
            child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            totalMinWidth += child.DesiredSize.Width;
            maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
        }

        if (totalMinWidth <= availableSize.Width)
            return new Size(availableSize.Width, maxHeight);
        
        double totalHeight = 0;
        
        foreach (var child in Children)
        {
            child.Measure(new Size(availableSize.Width, double.PositiveInfinity));
            totalHeight += child.DesiredSize.Height;
        }
        
        return new Size(availableSize.Width, totalHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double totalMinWidth = Children.Sum(c => c.DesiredSize.Width);

        if (totalMinWidth <= finalSize.Width)
        {
            double totalWeight = Children.Sum(GetPriorityWeight);
            double x = 0;

            foreach (var child in Children)
            {
                double weight = GetPriorityWeight(child);
                double width = finalSize.Width * (weight / totalWeight);
                child.Arrange(new Rect(x, 0, width, finalSize.Height));
                x += width;
            }

            return finalSize;
        }
        
        // Vertical Layout
        double y = 0;

        foreach (var child in Children)
        {
            double height = child.DesiredSize.Height;
            child.Arrange(new Rect(0, y, finalSize.Width, height));
            y += height;
        }

        return new Size(finalSize.Width, y);
    }

}
