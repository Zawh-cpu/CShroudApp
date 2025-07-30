using System.ComponentModel;
using Avalonia.Controls;

namespace CShroudApp.Desktop.Views;

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