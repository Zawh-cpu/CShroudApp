namespace Backend.Domain.Interfaces;

public interface IConfigManager
{
    Task SaveConfigAsync();
    
    event Action? ConfigChanged;
    void NotifyConfigChanged();
}