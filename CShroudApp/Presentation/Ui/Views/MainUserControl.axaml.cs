using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace CShroudApp.Presentation.Ui.Views;

public partial class MainUserControl : UserControl
{
    public MainUserControl()
    {
        InitializeComponent();
    }
    
    private void OnClosing(object? sender, CancelEventArgs e)
    {
    }
}