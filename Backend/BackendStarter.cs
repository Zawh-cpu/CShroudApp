using System.Text.Json;
using Backend.Application.Factories;
using Backend.Domain.Configs;
using Backend.Domain.Interfaces;
using Backend.Domain.JsonContexts;
using Backend.Domain.Utils;
using Backend.Infrastructure.Platforms.Android;
using Backend.Infrastructure.Platforms.Windows.Services;
using Backend.Infrastructure.Services;
using Backend.Infrastructure.StaticServices;
using Backend.Infrastructure.VpnCores.SingBox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend;

public static class BackendStarter
{
    private static IHost? _prestartedHost;
    
    public static IHost Start(string[] args, ServiceCollection? additionalServices)
    {
        if (_prestartedHost is not null) return _prestartedHost;
        
        var builder = new HostApplicationBuilder(args);
        builder.Logging.AddConsole();

        FileChecker.CheckFiles();
        
        builder.Configuration.AddJsonFile(AppConstants.ConfigFilePath, optional: true, reloadOnChange: false);
        builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection(nameof(ApplicationConfig)));
        
        Console.WriteLine(AppConstants.ConfigFilePath);
        
        var cfg = builder.Configuration
            .GetRequiredSection(nameof(ApplicationConfig))
            .Get<ApplicationConfig>()!;
        
        builder.Services.AddHttpClient("CrimsonShroudApiHook",
            client => client.BaseAddress = new Uri(cfg.Network.ReservedGatewayAddresses.First()));

        builder.Services.AddSingleton<ApplicationConfig>(cfg);
        builder.Services.AddSingleton<IConfigManager, ConfigManager>();
        builder.Services.AddSingleton<IEventManager, EventManager>();
        builder.Services.AddSingleton<IApiRepository, ApiRepository>();
        builder.Services.AddSingleton<IStorageManager, StorageManager>();
        builder.Services.AddSingleton<ISessionManager, SessionManager>();
        builder.Services.AddSingleton<INotificationManager, NotificationManager>();
        builder.Services.AddSingleton<IProcessManager, ProcessManager>();
        builder.Services.AddSingleton<ProcessFactory>();
        builder.Services.AddSingleton<IInternalDataManager, InternalDataManager>();
        builder.Services.AddSingleton<IVpnService, VpnService>();
        builder.Services.AddSingleton<IQuickAuthService, QuickAuthService>();

        builder.Services.AddSingleton<IVpnCore, SingBoxCore>();

        switch (PlatformInformation.GetPlatformRaw())
        {
            case Platform.Windows:
                builder.Services.AddSingleton<IProxyManager, WindowsProxyService>();
                builder.Services.AddSingleton<IToastManager, WindowsToastManager>();
                builder.Services.AddSingleton<IInstalledAppsManager, WindowsInstalledAppsManager>();
                break;
            case Platform.Android:
                builder.Services.AddSingleton<IProxyManager, AndroidProxyService>();
                builder.Services.AddSingleton<IToastManager, AndroidToastManager>();
                break;
            default:
                throw new NotSupportedException("Unsupported Platform");
        }
        
        if (additionalServices is not null)
            foreach (var serviceDescriptor in additionalServices)
                builder.Services.Add(serviceDescriptor);
        
        var app = builder.Build();
        //LocalizationService.CurrentLocalization = cfg.Localization;
        
        // Preheating important services
        _ = app.Services.GetService<ISessionManager>();
        
        _prestartedHost = app;
        return app;
    }
}