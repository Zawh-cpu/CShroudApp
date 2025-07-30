using System.IO;
using System.Text.Json;
using Backend.Domain.Configs;
using Backend.Domain.Interfaces;
using Backend.Domain.JsonContexts;
using Backend.Domain.Utils;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Services;

public class ConfigManager : IConfigManager
{
    public event Action? ConfigChanged;
    private readonly ApplicationConfig _applicationConfig;
    
    public ConfigManager(ApplicationConfig applicationConfig)
    {
        _applicationConfig = applicationConfig;
    }
    
    public void NotifyConfigChanged() => ConfigChanged?.Invoke();
    
    public async Task SaveConfigAsync()
    {
        FileChecker.CheckAndCreatePathToIfNotExists(AppConstants.ConfigFilePath);

        await File.WriteAllTextAsync(AppConstants.ConfigFilePath, JsonSerializer.Serialize(_applicationConfig, ConfigsJsonContext.Default.ApplicationConfig));
    }
}