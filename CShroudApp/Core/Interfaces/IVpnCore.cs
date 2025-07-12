﻿using Ardalis.Result;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

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
    public event Action<ulong, ulong>? SpeedUpdated;
}