using System.Collections.ObjectModel;
using System.Windows.Input;
using Ardalis.Result;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Entities;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.StaticServices;
using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels.Settings;

namespace CShroudApp.Presentation.Ui.ViewModels;

public partial class DashboardViewModel : ViewModelBase, IDisposable
{
    private readonly ISessionManager _sessionManager;
    private readonly IVpnService _vpnService;
    private readonly IStorageManager _storageManager;
    private readonly ApplicationConfig _config;
    private readonly IApiRepository _apiRepository;
    private readonly INotificationManager _notificationManager;
    private readonly IConfigManager _configManager;
    private readonly IToastManager _toastManager;
    private readonly INavigationService _navigationService;
    
    private DispatcherTimer _timer;
    
    [ObservableProperty]
    private string _timerText = "00:00:00";

    [ObservableProperty]
    private ulong _uploadSpeed = 0;
    
    [ObservableProperty]
    private ulong _downloadSpeed = 0;

    public class ModeItem
    {
        public required string Name { get; set; }
        public int? HttpPort { get; set; }
        public int? SocksPort { get; set; }
    }
    
    public ObservableCollection<KeyValuePair<VpnMode, ModeItem>> Modes { get; } = new();
    
    public IEnumerable<ModeItem> ModesForView => Modes.Select(x => x.Value);
    
    public ObservableCollection<Location> AvailableLocations { get; set; }= new();

    [ObservableProperty] private Location? _selectedLocation;
    
    public ICommand ToggleVpnCommand { get; }
    public ICommand GoToSettingsCommand { get; }

    public string CurrentIpAddress { get; set; } = "91.144.254.24";
    
    public DashboardViewModel(ISessionManager sessionManager, IVpnService vpnService, ApplicationConfig config, IStorageManager storageManager, IApiRepository apiRepository, INotificationManager notificationManager, IConfigManager configManager, IToastManager toastManager, INavigationService navigationService)
    {
        _sessionManager = sessionManager;
        _vpnService = vpnService;
        _config = config;
        _storageManager = storageManager;
        _apiRepository = apiRepository;
        _notificationManager = notificationManager;
        _configManager = configManager;
        _toastManager = toastManager;
        _navigationService = navigationService;
        
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.Tun, new ModeItem { Name = LocalizationService.Translate("VpnMode-Tun"), HttpPort = null, SocksPort = null }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.TunPlusProxy, new ModeItem { Name = LocalizationService.Translate("VpnMode-TunPlusProxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.Proxy, new ModeItem { Name = LocalizationService.Translate("VpnMode-Proxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        Modes.Add(new KeyValuePair<VpnMode, ModeItem>(VpnMode.TransparentProxy, new ModeItem { Name = LocalizationService.Translate("VpnMode-TransparentProxy"), HttpPort = (int)_config.Vpn.Inputs.Http.Port, SocksPort = (int)_config.Vpn.Inputs.Socks.Port }));
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        
        ToggleVpnCommand = new RelayCommand(() => Task.Run(ToggleVpn));
        GoToSettingsCommand = new RelayCommand(() => _navigationService.GoTo<GeneralSettingsViewModel>());

        SelectedMode = Modes.FirstOrDefault(x => x.Key == _config.Vpn.Mode).Value ?? Modes.FirstOrDefault(x => x.Key == VpnMode.Tun).Value;
        
        _vpnService.VpnEnabled += OnVpnEnabled;
        _vpnService.VpnDisabled += OnVpnDisabled;
        _vpnService.VpnStartedCancellation += OnVpnStartedCancellation;
        _vpnService.SpeedUpdated += SpeedUpdate;
        _configManager.ConfigChanged += OnConfigChanged;
        
        Task.Run(UpdateLocations);
    }

    public void SpeedUpdate(ulong upload, ulong download)
    {
        UploadSpeed = upload;
        DownloadSpeed = download;
    }
    
    public async Task UpdateLocations()
    {
        Location[]? res = _storageManager.GetValue<Location[]>("CachedLocations");
        if (res is null)
        {
            var request = await _apiRepository.GetAvailableLocationsAsync();
            if (!request.IsSuccess) return;
            res = request.Value;
            await _storageManager.SetValueAsync("CachedLocations", res, TimeSpan.FromMinutes(7), saveChanges: true);
        }
        
        if (res is null) return;
        
        AvailableLocations = new ObservableCollection<Location>(res);
        
        if (SelectedLocation is not null) return;
        
        var item = _storageManager.GetValue<Location>("SelectedLocation");
        if (item is not null)
            SelectedLocation = AvailableLocations.FirstOrDefault(x => x.City == item.City && x.Country == item.Country);
        
        if (SelectedLocation is null && AvailableLocations.Any()) SelectedLocation = AvailableLocations.First();
    }
    
    public async Task ToggleVpn()
    {
        if (_vpnService.IsRunning)
        {
            await _vpnService.DisableAsync();
        }
        else
        {
            await StartVpn(SelectedLocation);
        }
    }

    public async Task StartVpn(Location? location)
    {
        if (_vpnService.IsRunning) return;
        VpnConnectionCredentials? credentials = _storageManager.GetValue<VpnConnectionCredentials>("VpnConnectionCredentials");
        if (credentials is null)
        {
            if (location is null)
            {
                _notificationManager.AddNotification(new NotificationObject()
                {
                    Title = LocalizationService.Translate("VpnNetwork-NoSelectedLocation"),
                    Message = LocalizationService.Translate("VpnNetwork-NoSelectedLocation-Text"),
                    Type = NotificationType.Error
                });
                return;
            }
                
            var temp = await _apiRepository.TryConnectToVpnNetworkAsync(_vpnService.SupportedProtocols,
                location.City);
            if (temp.IsSuccess) credentials = temp.Value;
        }
            
        if (credentials is null)
        {
            _notificationManager.AddNotification(new NotificationObject()
            {
                Title = LocalizationService.Translate("VpnNetwork-ErrorConnection"),
                Message = LocalizationService.Translate("VpnNetwork-ErrorConnection-Text"),
                Type = NotificationType.Error
            });
                
            return;
        }

        await _vpnService.EnableAsync(_config.Vpn.Mode, credentials);
    }

    private void OnVpnEnabled(object? sender, EventArgs e)
    {
        Console.WriteLine("Connection started");
        _timer.Start();
        _toastManager.SendToast();
    }

    private void OnVpnDisabled(object? sender, EventArgs e)
    {
        Console.WriteLine("Connection closed");
        _timer.Stop();
        TimerText = "00:00:00";
    }

    private void OnVpnStartedCancellation(Result<object> result)
    {
        switch (result.Status)
        {
            case ResultStatus.Unauthorized:
                _notificationManager.AddNotification(new NotificationObject()
                {
                    Title = LocalizationService.Translate("Error-InsufficientRights"),
                    Message = LocalizationService.Translate("VpnService-AdminRightsRequired-Text"),
                    Type = NotificationType.Error
                });
                break;
            case ResultStatus.NotFound:
                _notificationManager.AddNotification(new NotificationObject()
                {
                    Title = LocalizationService.Translate("Error-CoreFileNotFound"),
                    Message = LocalizationService.Translate("Error-CoreFileNotFound-Text"),
                    Type = NotificationType.Error
                });
                break;
            default:
                _notificationManager.AddNotification(new NotificationObject()
                {
                    Title = LocalizationService.Translate("Error-Unknown"),
                    Message = LocalizationService.Translate("Error-Unknown-Text"),
                    Type = NotificationType.Error
                });
                break;
        }
    }
    
    private void Timer_Tick(object? sender, EventArgs e)
    {
        var remaining = DateTime.UtcNow - _vpnService.SessionStartTime;

        if (remaining is null || remaining <= TimeSpan.Zero)
        {
            TimerText = "00:00:00";
            _timer.Stop();
        }
        else
        {
            TimerText = $"{remaining.Value.Hours:D2}:{remaining.Value.Minutes:D2}:{remaining.Value.Seconds:D2}";
        }
    }
    
    [ObservableProperty]
    private ModeItem? _selectedMode;

    partial void OnSelectedModeChanged(ModeItem? value)
    {
        if (!_isShowedNow || value is null) return;
        
        var key = Modes.FirstOrDefault(x => x.Value.Name == value.Name).Key;

        _config.Vpn.Mode = key;
        Task.Run(async () =>
        {
            _ = _configManager.SaveConfigAsync();
            if (_vpnService.IsRunning)
            {
                await _vpnService.DisableAsync();
                await StartVpn(SelectedLocation);
            }
        });
        
        _configManager.NotifyConfigChanged();
    }

    partial void OnSelectedLocationChanged(Location? value)
    {
        if (!_isShowedNow || value is null) return;
        
        _storageManager.SetValueAsync("SelectedLocation", value);

        Task.Run(async () =>
        {
            _ = _configManager.SaveConfigAsync();
            if (_vpnService.IsRunning)
            {
                await _vpnService.DisableAsync();
                await StartVpn(value);
            }
        });
    }
    
    public void OnConfigChanged()
    {
        var mode = Modes.FirstOrDefault(x => x.Key == _config.Vpn.Mode).Value;
        if (mode is null) return;

        SelectedMode = mode;
    }
    
    public void Dispose()
    {
        _vpnService.VpnEnabled -= OnVpnEnabled;
        _vpnService.VpnDisabled -= OnVpnDisabled;
        _vpnService.VpnStartedCancellation -= OnVpnStartedCancellation;
        _vpnService.SpeedUpdated -= SpeedUpdate;
        _configManager.ConfigChanged -= OnConfigChanged;
    }
}