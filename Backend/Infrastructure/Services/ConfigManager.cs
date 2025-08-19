using System.IO;
using System.Text.Json;
using System.Threading;
using Backend.Domain.Configs;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Domain.JsonContexts;
using Backend.Domain.Utils;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Services;

public class ConfigManager : IConfigManager
{
    public event Action? ConfigChanged;
    private readonly ApplicationConfig _applicationConfig;
    private Task _saveConfigTask = Task.CompletedTask;
    private Debouncer _debouncer = new();
    
    public ConfigManager(ApplicationConfig applicationConfig)
    {
        _applicationConfig = applicationConfig;
        
        _applicationConfig.PropertyChanged += (sender, args) =>
        {
            NotifyConfigChanged();
            _debouncer.Debounce(async void () => await SaveConfigAsync(), 2000);
        };
        
    }
    
    public void NotifyConfigChanged() => ConfigChanged?.Invoke();
    
    public async Task SaveConfigAsync()
    {
        FileChecker.CheckAndCreatePathToIfNotExists(AppConstants.ConfigFilePath);
        await ParseAndWriteConfigAsync(AppConstants.ConfigFilePath, _applicationConfig);
    }

    public static void ParseAndWriteConfig(string path, ApplicationConfig config)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(new AllInConfigStructure { ApplicationConfig = config }, ConfigsJsonContext.Default.AllInConfigStructure));
    }
    
    public static async Task ParseAndWriteConfigAsync(string path, ApplicationConfig config)
    {
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(new AllInConfigStructure { ApplicationConfig = config }, ConfigsJsonContext.Default.AllInConfigStructure));
    }
}