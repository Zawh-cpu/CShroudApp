﻿using System.Text.Json;
using CShroudApp.Application.Factories;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Interfaces;
using CShroudApp.Core.JsonContexts;
using CShroudApp.Core.Utils;
using CShroudApp.Infrastructure.Platforms.Android;
using CShroudApp.Infrastructure.Platforms.Windows.Services;
using CShroudApp.Infrastructure.Services;
using CShroudApp.Infrastructure.StaticServices;
using CShroudApp.Infrastructure.VpnCores.SingBox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CShroudApp;

public static class BackendStarter
{
    private static IHost? PrestartedHost;
    
    public static IHost Start(string[] args, ServiceCollection? additionalServices)
    {
        if (PrestartedHost is not null) return PrestartedHost;
        
        var builder = new HostApplicationBuilder(args);
        builder.Logging.AddConsole();

        ApplicationConfig cfg;
        try
        {
            cfg = JsonSerializer.Deserialize<ApplicationConfig>(File.ReadAllText(AppConstants.ConfigFilePath),
                ConfigsJsonContext.Default.ApplicationConfig)!;
        }
        catch(Exception)
        {
            cfg = new ApplicationConfig();
        }

        Console.WriteLine(AppConstants.ConfigFilePath);
        
        builder.Services.AddHttpClient("CrimsonShroudApiHook",
            client => client.BaseAddress = new Uri(cfg.Network.ReservedGatewayAddresses.First()));

        builder.Services.AddSingleton<ApplicationConfig>(cfg);
        builder.Services.AddSingleton<IConfigManager, ConfigManager>();
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
        LocalizationService.CurrentLocalization = cfg.Localization;
        
        PrestartedHost = app;
        return app;
    }
}