using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CShroudApp.Desktop.ViewModels.MainPages;


public partial class DashboardViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType => MainPagesType.Dashboard;
    public override string Title => "Dashboard";
    public override string Description => "Control your VPN connection and view status";
    
    public MainSharedMemory SharedMemory { get; set; }
    
    private readonly IApiRepository _apiRepository;
    
    public bool IsConnectedToNetwork => SharedMemory.VpnService.IsConnected;
    
    [ObservableProperty]
    private ulong _uploadSpeed = 0;
    
    [ObservableProperty]
    private ulong _downloadSpeed = 0;

    public DashboardViewModel(MainSharedMemory sharedMemory, IApiRepository apiRepository)
    {
        SharedMemory = sharedMemory;
        _apiRepository = apiRepository;
        
        SharedMemory.EventManager.OnConnectedToNetworkSuccessfully += () => OnPropertyChanged(nameof(IsConnectedToNetwork));
        SharedMemory.EventManager.OnFailedConnectToNetwork += () => OnPropertyChanged(nameof(IsConnectedToNetwork));
        SharedMemory.VpnService.SpeedUpdated += SpeedUpdate;
    }

    public void SpeedUpdate(ulong upload, ulong download)
    {
        UploadSpeed = upload;
        DownloadSpeed = download;
    }
    
    [RelayCommand]
    private async Task ToggleConnectionToNetwork()
    {
        Console.WriteLine($"ToggleConnectionToNetwork: {SharedMemory.VpnService.IsConnected}");
        if (SharedMemory.VpnService.IsConnected)
        {
            await SharedMemory.VpnService.DisableAsync();
            return;
        }

        // MAKE OTHER CALLS TO UNOFFICIAL
        if (SharedMemory.SelectedNetworkShard is null) return;
        
        var credentials = await _apiRepository.TryConnectToVpnNetworkAsync([VpnProtocol.Vless], SharedMemory.SelectedNetworkShard.Name);
        if (!credentials.IsSuccess)
        {
            SharedMemory.EventManager.FailedConnectToNetwork();
            return;
        }
        
        await SharedMemory.VpnService.EnableAsync(SharedMemory.ApplicationConfig.Vpn.Mode, credentials.Value);
    }
}