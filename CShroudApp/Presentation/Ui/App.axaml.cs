using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CShroudApp.Core.Interfaces;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.MarkupExtensions;
using CShroudApp.Presentation.Ui.Services;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.ViewModels.Auth;
using CShroudApp.Presentation.Ui.ViewModels.Settings;
using CShroudApp.Presentation.Ui.Views;
using CShroudApp.Presentation.Ui.Views.Auth;
using CShroudApp.Presentation.Ui.Views.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Presentation.Ui;


public partial class App : Avalonia.Application
{
    private IVpnService? _vpnService;

    public static ServiceCollection GetUiDependencyCollection()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton<INavigationService, NavigationService>();
        
        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<LoginViewModel>();
        collection.AddSingleton<QuickLoginViewModel>();
        collection.AddSingleton<DashboardViewModel>();
        collection.AddSingleton<AppViewModel>();
        
        collection.AddSingleton<GeneralSettingsViewModel>();
        
        return collection;
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var host = BackendStarter.Start([], GetUiDependencyCollection());
        
        _vpnService = host.Services.GetRequiredService<IVpnService>();
        
        DataContext = host.Services.GetRequiredService<AppViewModel>();
        var vm = host.Services.GetService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainView()
            {
                DataContext = vm
            };
                
            Console.WriteLine("Apps exit configured");
            //desktop.Exit += OnApplicationExit;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            if (Design.IsDesignMode)
            {
                singleViewPlatform.MainView = new MainUserControl()
                {
                    DataContext = new DesignerViewModel()
                };
            }
            else
            {
                singleViewPlatform.MainView = new MainUserControl()
                {
                    DataContext = vm
                };
            }
        }
            
        //AppDomain.CurrentDomain.ProcessExit += OnEnvironmentExit;
            
        base.OnFrameworkInitializationCompleted();
    }

    [RequiresUnreferencedCode("Disabling Avalonia other validation")]
    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private void OnEnvironmentExit(object? sender, EventArgs e)
    {
        if (_vpnService is not null && _vpnService.IsRunning)
            Task.WaitAll(_vpnService.DisableAsync());
    }

    private void NativeTrayMenu_OnOpening(object? sender, EventArgs e)
    {
        Console.WriteLine("FWFWEFWEFWEFWFEWFEWFWEFEWFEWFEWFEWFFWEFWEFWF");
    }
}