using System.IO;
using System.Text.Json;
using Backend.Domain.Configs;
using Backend.Domain.JsonContexts;
using Backend.Infrastructure.Services;

namespace Backend.Domain.Utils;

public static class FileChecker
{
    public static bool CheckAndCreatePathToIfNotExists(string path)
    {
        if (Path.Exists(path)) return false;
        
        var dirPath = Path.GetDirectoryName(path);
        if (dirPath is not null && !Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        
        return true;
    }
    
    public static void CheckFiles()
    {
        if (CheckAndCreatePathToIfNotExists(AppConstants.ConfigFilePath))
            ConfigManager.ParseAndWriteConfig(AppConstants.ConfigFilePath, new ApplicationConfig());
    }
}