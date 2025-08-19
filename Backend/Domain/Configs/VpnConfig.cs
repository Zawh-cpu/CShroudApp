using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Backend.Domain.Entities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Backend.Domain.Configs;

public class VpnConfig : INotifyPropertyChanged
{
    private VpnMode _mode = VpnMode.Proxy;
    [JsonConverter(typeof(JsonStringEnumConverter<VpnMode>))]
    public VpnMode Mode { get => _mode; set => SetField(ref _mode, value); }


    private VpnCore _core = VpnCore.SingBox;

    [JsonConverter(typeof(JsonStringEnumConverter<VpnCore>))]
    public VpnCore Core { get => _core; set => SetField(ref _core, value); }

    
    private bool _reverseMode = false;
    public bool ReverseMode { get => _reverseMode; set => SetField(ref _reverseMode, value); }

    
    private bool _savePreviousProxy = true;
    public bool SavePreviousProxy { get => _savePreviousProxy; set => SetField(ref _savePreviousProxy, value); }

    
    private string _preferredProxy = "8.8.8.8";
    public string PreferredProxy { get => _preferredProxy; set => SetField(ref _preferredProxy, value); }

    
    private SplitTunnelingConfig _splitTunneling = new();
    public SplitTunnelingConfig SplitTunneling { get => _splitTunneling; set => SetField(ref _splitTunneling, value); }

    private InputsObject _inputs = new();
    public InputsObject Inputs { get => _inputs; set => SetField(ref _inputs, value); }

    public class InputsObject : INotifyPropertyChanged
    {
        public class InputObj : INotifyPropertyChanged
        {
            private string _host = string.Empty;
            public string Host { get => _host; set => SetField(ref _host, value); }

            private uint _port = uint.MaxValue;
            public uint Port { get => _port; set => SetField(ref _port, value); }
            
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

        private ProxyProtocol _preferredInput = ProxyProtocol.Socks;

        [JsonConverter(typeof(JsonStringEnumConverter<ProxyProtocol>))]
        public ProxyProtocol PreferredInput { get => _preferredInput; set => SetField(ref _preferredInput, value); }

        private string[] _excludeProxyForAddresses = [];
        public string[] ExcludeProxyForAddresses { get => _excludeProxyForAddresses; set => SetField(ref _excludeProxyForAddresses, value); }

        
        private InputObj _http = new() { Host = "127.0.0.1", Port = 10808 };
        public InputObj Http { get => _http; set => SetField(ref _http, value); }

        
        private InputObj _socks = new() { Host = "127.0.0.1", Port = 10809 };
        public InputObj Socks { get => _socks; set => SetField(ref _socks, value); }

        public InputsObject()
        {
            Hook(Http);
            Hook(Socks);
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
            nested.PropertyChanged += (_, __) => OnPropertyChanged(nameof(ApplicationConfig));
        }
    }

    private void Hook(INotifyPropertyChanged nested)
    {
        nested.PropertyChanged += (_, __) => OnPropertyChanged(nameof(VpnConfig));
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
    
    public VpnConfig()
    {
        Hook(SplitTunneling);
        Hook(Inputs);
    }
}