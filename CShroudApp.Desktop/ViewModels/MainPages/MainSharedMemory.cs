using System.Collections.ObjectModel;
using Backend.Domain.Configs;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Infrastructure.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace CShroudApp.Desktop.ViewModels.MainPages;

public partial class UiCachedOptions: ObservableObject
{
    [ObservableProperty]
    private bool _settingsIsSplitTunnelingAppsSelectorCollapsed = false;
    
    [ObservableProperty]
    private NetworkShard? _selectedNetworkShard;
}

public class NetworkShard
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    
    public required bool IsOfficial { get; set; }
    public required bool DoSupportVpn { get; set; }
    
    public required uint AveragePing { get; set; }
}

public partial class MainSharedMemory : ObservableObject
{
    public ObservableCollection<NetworkShard> AvailableServers { get; set; } =
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
    private NetworkShard? _selectedNetworkShard;
    
    [ObservableProperty]
    private UiCachedOptions _uiCachedOptions;
    
    public ApplicationConfig ApplicationConfig { get; set; }
    
    partial void OnSelectedNetworkShardChanged(NetworkShard? value)
    {
        SelectedNetworkShard = value;

        if (value is not null)
            _uiCachedOptions.SelectedNetworkShard = SelectedNetworkShard;
    }

    private Debouncer _uiCachedOptionsDebouncer = new();
    
    public IVpnService VpnService { get; }
    public IEventManager EventManager { get; }
    
    public bool IsConnectedToNetwork => VpnService.IsConnected;
    
    public MainSharedMemory(ApplicationConfig applicationConfig, IStorageManager storageManager, IVpnService vpnService, IEventManager eventManager)
    {   
        ApplicationConfig = applicationConfig;
        VpnService = vpnService;
        EventManager = eventManager;

        var value = storageManager.GetValue<UiCachedOptions>("SharedMemoryUiCachedOptions");
        _uiCachedOptions = value ?? new UiCachedOptions();
        
        _uiCachedOptions.PropertyChanged += (s, e) => _uiCachedOptionsDebouncer.Debounce(
            async void () => await storageManager.SetValueAsync("SharedMemoryUiCachedOptions", _uiCachedOptions, saveChanges: true), 2000);
        
        if (_uiCachedOptions.SelectedNetworkShard is not null)
            SelectedNetworkShard = _uiCachedOptions.SelectedNetworkShard;
        
        VpnService.VpnEnabled += (obj, e) => OnPropertyChanged(nameof(IsConnectedToNetwork));
        VpnService.VpnDisabled += (obj, e) => OnPropertyChanged(nameof(IsConnectedToNetwork));
    }
}