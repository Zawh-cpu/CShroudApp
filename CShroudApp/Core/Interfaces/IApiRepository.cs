﻿using System.Threading.Tasks;
using Ardalis.Result;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.Interfaces;

public interface IApiRepository
{
    string? RefreshToken { get; set; }
    string? ActionToken { get; set; }
    
    Task<Result<SignInDto>> SignInAsync(string username, string password);
    Task<Result<QuickAuthSessionDto>> BeginQuickAuthSessionAsync();
    Task<Result<SignInDto>> FinalizeQuickAuthSessionAsync(QuickAuthDto data);
    Task<Result<ActionTokenRefreshDto>> RefreshActionTokenAsync(string refreshToken);
    Task<Result<GetUserDto>> GetUserInformationAsync();
    Task<Result<Location[]>> GetAvailableLocationsAsync();

    Task<Result<VpnConnectionCredentials>> TryConnectToVpnNetworkAsync(VpnProtocol[] supportedProtocols,
        string location);
    
    Task Test();
}