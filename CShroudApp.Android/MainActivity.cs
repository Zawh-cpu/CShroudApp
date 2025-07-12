using System;
using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using CShroudApp.Presentation.Ui;

namespace CShroudApp.Android;

[Activity(
    Label = "CShroudApp.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{

    public MainActivity() : base()
    {
        Console.WriteLine("MainActivity Constructorfwefwefwefwewefwefwefewfewfwefwe");
        BackendStarter.Start([], App.GetUiDependencyCollection());
    }
    
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}