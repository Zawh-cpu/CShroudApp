using Ardalis.Result;
using Backend.Domain.Entities;
using Backend.Infrastructure.Services;

namespace Backend.Domain.Interfaces;

public interface IVpnService
{
    VpnProtocol[] SupportedProtocols { get; }
    
    Task<Result> EnableAsync(VpnMode mode, VpnConnectionCredentials credentials);
    Task DisableAsync();
    
    Task RestartAsync(VpnMode mode, VpnConnectionCredentials credentials);
    
    bool IsRunning { get; }
    bool IsConnected { get; }
    
    event EventHandler? VpnEnabled;
    event EventHandler? VpnDisabled;
    event Action<Result<object>>? VpnStartedCancellation;
    public event Action<ulong, ulong>? SpeedUpdated;

    public NetworkSession? Session { get; }

    public ulong Upload { get; }
    public ulong Download { get; }
    public uint Ping { get; }
}