using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Backend.Domain.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Backend.Domain.Configs;

public class ApplicationConfig : INotifyPropertyChanged
{
    private GeneralSettingsConfig _generalSettingsConfig = new();
    public GeneralSettingsConfig GeneralSettings { get => _generalSettingsConfig; set => SetField(ref _generalSettingsConfig, value); }

    
    private Localization _localization = Localization.English;
    
    [JsonConverter(typeof(JsonStringEnumConverter<Localization>))]
    public Localization Localization { get => _localization; set => SetField(ref _localization, value); }


    private LogLevelMode _logLevel = LogLevelMode.Off;
    [JsonConverter(typeof(JsonStringEnumConverter<LogLevelMode>))]
    public LogLevelMode LogLevel  { get => _logLevel; set => SetField(ref _logLevel, value); }
    
    
    private NetworkConfig _network = new();
    public NetworkConfig Network  { get => _network; set => SetField(ref _network, value); }

    
    private VpnConfig _vpn = new();
    public VpnConfig Vpn { get => _vpn; set => SetField(ref _vpn, value); }
    
    
    private DeveloperConfig _developerSettings = new();
    public DeveloperConfig DeveloperSettings { get => _developerSettings; set => SetField(ref _developerSettings, value); }

    
    private void Hook(INotifyPropertyChanged nested)
    {
        nested.PropertyChanged += (_, __) => OnPropertyChanged(nameof(ApplicationConfig));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public ApplicationConfig()
    {
        Hook(GeneralSettings);
        Hook(Vpn);
        Hook(Network);
        Hook(DeveloperSettings);
    }
}
