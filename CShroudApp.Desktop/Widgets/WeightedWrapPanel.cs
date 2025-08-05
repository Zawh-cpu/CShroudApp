using Avalonia;
using Avalonia.Controls;

namespace CShroudApp.Desktop.Widgets;

public class WeightedWrapPanel : Panel
{
    public static readonly AttachedProperty<double> WeightProperty =
        AvaloniaProperty.RegisterAttached<WeightedWrapPanel, Control, double>("Weight", 1);

    public static void SetWeight(Control control, double value) => control.SetValue(WeightProperty, value);
    public static double GetWeight(Control control) => control.GetValue(WeightProperty);

    public double ItemSpacing { get; set; } = 12;
    public double LineSpacing { get; set; } = 12;

    protected override Size MeasureOverride(Size availableSize)
    {
        var currentLine = new List<Control>();
        var totalWeight = 0.0;
        var currentWidth = 0.0;
        var maxLineHeight = 0.0;

        foreach (var child in Children)
        {
            var weight = GetWeight(child);
            currentLine.Add(child);
            totalWeight += weight;

            if (Children.IndexOf(child) == Children.Count - 1 || 
                currentWidth + availableSize.Width * weight / totalWeight + ItemSpacing > availableSize.Width)
            {
                foreach (var lineChild in currentLine)
                    lineChild.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                currentLine.Clear();
                totalWeight = 0;
                currentWidth = 0;
                maxLineHeight += Children.Max(c => c.DesiredSize.Height) + LineSpacing;
            }
        }

        return new Size(availableSize.Width, maxLineHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var currentLine = new List<(Control control, double weight)>();
        var lineTop = 0.0;
        var currentLineWidth = 0.0;

        foreach (var child in Children)
        {
            var weight = GetWeight(child);
            currentLine.Add((child, weight));
            currentLineWidth += weight;

            double totalSpacing = (currentLine.Count - 1) * ItemSpacing;
            double requiredWidth = finalSize.Width * (currentLineWidth / currentLine.Sum(c => c.weight)) + totalSpacing;

            if (requiredWidth > finalSize.Width)
            {
                ArrangeLine(currentLine.SkipLast(1).ToList(), finalSize.Width, lineTop);
                lineTop += currentLine.Max(x => x.control.DesiredSize.Height) + LineSpacing;
                currentLine = [(child, weight)];
                currentLineWidth = weight;
            }
        }

        if (currentLine.Count > 0)
        {
            ArrangeLine(currentLine, finalSize.Width, lineTop);
        }

        return finalSize;
    }

    private void ArrangeLine(List<(Control control, double weight)> line, double totalWidth, double top)
    {
        double totalWeight = line.Sum(x => x.weight);
        double x = 0;

        foreach (var (control, weight) in line)
        {
            double width = (totalWidth - (line.Count - 1) * ItemSpacing) * (weight / totalWeight);
            control.Arrange(new Rect(x, top, width, control.DesiredSize.Height));
            x += width + ItemSpacing;
        }
    }
}
