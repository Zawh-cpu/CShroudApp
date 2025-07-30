using System.IO;
using System.Text.Json;
using Backend.Domain.Configs;
using Backend.Domain.JsonContexts;

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
            File.WriteAllText(AppConstants.ConfigFilePath, JsonSerializer.Serialize(new ApplicationConfig(), ConfigsJsonContext.Default.ApplicationConfig));
    }
}