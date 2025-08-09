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
    
    public static readonly StyledProperty<uint> RowSpacingProperty =
        AvaloniaProperty.Register<AdaptiveGridPanel, uint>(nameof(RowSpacing));
    
    public uint RowSpacing
    {
        get => GetValue(RowSpacingProperty);
        set => SetValue(RowSpacingProperty, value);
    }
    
    public static readonly StyledProperty<uint> ColumnSpacingProperty =
        AvaloniaProperty.Register<AdaptiveGridPanel, uint>(nameof(ColumnSpacing));
    
    public uint ColumnSpacing
    {
        get => GetValue(ColumnSpacingProperty);
        set => SetValue(ColumnSpacingProperty, value);
    }
    
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
        
        var spaceXAdditional = (Children.Count - 1) * RowSpacing;

        if (totalMinWidth + spaceXAdditional <= availableSize.Width)
            return new Size(availableSize.Width + spaceXAdditional, maxHeight);
        
        double totalHeight = 0;
        
        foreach (var child in Children)
        {
            child.Measure(new Size(availableSize.Width, double.PositiveInfinity));
            totalHeight += child.DesiredSize.Height;
        }
        
        return new Size(availableSize.Width, totalHeight + ColumnSpacing * (Children.Count - 1));
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double totalMinWidth = Children.Sum(c => c.DesiredSize.Width);
        var spaceXAdditional = (Children.Count - 1) * RowSpacing;
        
        if (totalMinWidth + spaceXAdditional <= finalSize.Width)
        {
            double totalWeight = Children.Sum(GetPriorityWeight);
            double remainingWidth = finalSize.Width - totalMinWidth - spaceXAdditional;
            double x = 0;

            foreach (var child in Children)
            {
                double weight = GetPriorityWeight(child);
                double width = child.DesiredSize.Width;
                
                if (remainingWidth > 0)
                {
                    double extraWidth = remainingWidth * (weight / totalWeight);
                    width += extraWidth;
                }

                child.Arrange(new Rect(x, 0, width, finalSize.Height));
                x += width + RowSpacing;
            }

            return finalSize;
        }
        
        // Vertical Layout
        double y = 0;
        
        foreach (var child in Children)
        {
            double height = child.DesiredSize.Height;
            child.Arrange(new Rect(0, y, finalSize.Width, height));
            y += height + ColumnSpacing;
        }

        return new Size(finalSize.Width, y);
    }

}
