using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Backend.Domain.Configs;

public class NetworkConfig : INotifyPropertyChanged
{
    private List<string> _reservedGatewayAddresses = [ "http://localhost:5234" ];
    public List<string> ReservedGatewayAddresses { get => _reservedGatewayAddresses; set => SetField(ref _reservedGatewayAddresses, value); }

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