using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Backend.Domain.Configs;

public enum SplitTunnelingRuleType
{
    Process,
    Host,
    Ip,
    Port,
    Path
}

public partial class SplitTunnelingRule
{
    /*[ObservableProperty] private bool _enabled = true;
    
    [ObservableProperty] private string _name = string.Empty;
    
    [ObservableProperty] private string _value = string.Empty;
    
    [ObservableProperty] private SplitTunnelingRuleType _type = SplitTunnelingRuleType.Process;

    [ObservableProperty] private bool _isCustom;*/
    
    public bool Enabled { get; set; } = true;
    
    public string Name { get; set; } = string.Empty;
    
    public string Value { get; set; } = string.Empty;
    
    [JsonConverter(typeof(JsonStringEnumConverter<SplitTunnelingRuleType>))]
    public SplitTunnelingRuleType Type { get; set; } = SplitTunnelingRuleType.Process;

    public bool IsCustom { get; set; }
}

public class SplitTunnelingConfig : INotifyPropertyChanged
{
    private bool _enabled;
    public bool Enabled { get => _enabled; set => SetField(ref _enabled, value); }
    
    private bool _reverseMode = false;
    public bool ReverseMode { get => _reverseMode; set => SetField(ref _reverseMode, value); }
    
    public ObservableCollection<SplitTunnelingRule> Rules { get; set; } = [];

    public SplitTunnelingConfig()
    {
        Rules.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Rules));
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
    
    private void Hook(INotifyPropertyChanged nested)
    {
        nested.PropertyChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(SplitTunnelingConfig));
        };
    }
}