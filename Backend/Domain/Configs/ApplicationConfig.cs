using System.Text.Json.Serialization;
using Backend.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Backend.Domain.Configs;

public class ApplicationConfig
{
    [JsonConverter(typeof(JsonStringEnumConverter<Localization>))]
    public Localization Localization { get; set; } = Localization.English;
    
    [JsonConverter(typeof(JsonStringEnumConverter<LogLevelMode>))]
    public LogLevelMode LogLevel { get; set; } = LogLevelMode.Off;
    public NetworkConfig Network { get; set; } = new();
    
    public VpnConfig Vpn { get; set; } = new();
    
    public DeveloperConfig DeveloperSettings { get; set; } = new();
}
