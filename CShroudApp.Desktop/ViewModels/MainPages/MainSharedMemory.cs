using System.Collections.ObjectModel;
using Backend.Domain.Configs;
using Backend.Infrastructure.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CShroudApp.Desktop.ViewModels.MainPages;

public class Server
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    
    public required bool IsOfficial { get; set; }
    public required bool DoSupportVpn { get; set; }
    
    public required uint AveragePing { get; set; }
}

public partial class MainSharedMemory : ObservableObject
{
    public ObservableCollection<Server> AvailableServers { get; set; } =
    [
        new()
        {
            Name = "city-newyork",
            Country = "us",
            IsOfficial = true,
            DoSupportVpn = true,
            AveragePing = 110
        },
        new()
        {
            Name = "city-frankfurt",
            Country = "de",
            IsOfficial = true,
            DoSupportVpn = true,
            AveragePing = 28
        },
    ];
    
    [ObservableProperty]
    private Server? _selectedServer;
    
    public ApplicationConfig ApplicationConfig { get; set; }
    
    partial void OnSelectedServerChanged(Server? value)
    {
        SelectedServer = value;
    }

    public MainSharedMemory(ApplicationConfig applicationConfig)
    {   
        ApplicationConfig = applicationConfig;
    }
}