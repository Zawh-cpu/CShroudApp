using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Backend.Domain.Configs;

public class DeveloperConfig : INotifyPropertyChanged
{
    private uint _clashApiPort = 10555;
    public uint ClashApiPort { get => _clashApiPort; set => SetField(ref _clashApiPort, value); }
    
    
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
}