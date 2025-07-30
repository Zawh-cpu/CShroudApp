using Ardalis.Result;
using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces;

public interface IVpnService
{
    VpnProtocol[] SupportedProtocols { get; }
    
    Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials);
    Task DisableAsync();
    
    Task RestartAsync(VpnMode mode, VpnConnectionCredentials credentials);
    
    bool IsRunning { get; }
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
    event Action<Result<object>>? VpnStartedCancellation;
    public event Action<ulong, ulong>? SpeedUpdated;
    
    public DateTime? SessionStartTime { get; }
    
    public ulong Upload { get; }
    public ulong Download { get; }
}