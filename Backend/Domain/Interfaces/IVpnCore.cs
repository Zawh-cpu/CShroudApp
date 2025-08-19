using Ardalis.Result;
using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IVpnCore
{
    public event EventHandler? CoreEnabled;
    public event EventHandler? CoreDisabled;
    
    bool IsRunning { get; }
    VpnProtocol[] SupportedProtocols { get; }
    VpnProtocol[] AutoSetInboundSupportedProtocol { get; }
    bool DoNeedElevationForTun { get; }
    
    Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials);
    Task DisableAsync();
    
    public ulong Upload { get; set; }
    public ulong Download { get; set; }
    public uint Ping { get; set; }
    public event Action<ulong, ulong>? SpeedUpdated;
}