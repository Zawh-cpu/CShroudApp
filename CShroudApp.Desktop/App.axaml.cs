using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Backend;
using Backend.Domain.Interfaces;
using CShroudApp.Desktop.Interfaces;
using CShroudApp.Desktop.Resources.Panels.Auth.ViewModels;
using CShroudApp.Desktop.Services;
using CShroudApp.Desktop.ViewModels;
using CShroudApp.Desktop.ViewModels.MainPages;
using CShroudApp.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Desktop;


public partial class App : Avalonia.Application
{
    private IVpnService? _vpnService;

    public static ServiceCollection GetUiDependencyCollection()
    {
        var collection = new ServiceCollection();
        collection.AddSingleton<INavigationService, NavigationService>();
        
        collection.AddSingleton<AppViewModel>();
        collection.AddSingleton<MainWindowViewModel>();
        
        collection.AddSingleton<LoginViewModel>();
        collection.AddSingleton<DefaultLoginPanelViewModel>();
        collection.AddSingleton<TelegramQuickLoginPanelViewModel>();

        collection.AddSingleton<MainViewModel>();
        collection.AddSingleton<MainSharedMemory>();
        collection.AddSingleton<DashboardViewModel>();
        collection.AddSingleton<ServersViewModel>();
        
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
        var vm = host.Services.GetService<MainWindowViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = vm
            };
                
            Console.WriteLine("Apps exit configured");
            //desktop.Exit += OnApplicationExit;
        } else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            if (Design.IsDesignMode)
            {
                singleViewPlatform.MainView = new MainUserControl()
                {
                    DataContext = vm
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
}