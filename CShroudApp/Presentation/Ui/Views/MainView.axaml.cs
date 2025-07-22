using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace CShroudApp.Presentation.Ui.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        this.Closing += OnClosing;
    }
    
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        this.Hide();
    }
}