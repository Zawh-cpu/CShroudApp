﻿using System.Net.Http;
using System.Threading;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace CShroudApp.Infrastructure.Services;

public class QuickAuthService : IQuickAuthService
{
    private HubConnection _connection;
    private readonly IApiRepository _apiRepository;
    
    public event Action? OnAttemptDeclined;
    public event EventHandler<SignInDto>? OnAttemptSuccess;
    public event EventHandler<QuickAuthSessionDto>? OnSessionCreated;
    public event Action? OnSessionFailed;

    public QuickAuthService(IHttpClientFactory httpClientFactory, IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
        
        _connection = new HubConnectionBuilder()
            .WithUrl(httpClientFactory.CreateClient("CrimsonShroudApiHook").BaseAddress + "api/v1/quick-auth-hub")
            .Build();
        
        _connection.On<QuickAuthDto>("OnStatusChanged", OnStatusChanged);
    }

    public async Task RunSession(CancellationToken cancellationToken)
    {
        var session = await _apiRepository.BeginQuickAuthSessionAsync();
        if (!session.IsSuccess)
        {
            OnSessionFailed?.Invoke();
            return;
        }
        
        OnSessionCreated?.Invoke(this, session);
        
        Console.WriteLine("Session created -- EUW");
        
        await _connection.StartAsync(cancellationToken);
        await _connection.InvokeAsync("SubscribeToSession", session.Value.SessionId);
    }

    private async Task OnStatusChanged(QuickAuthDto data)
    {
        await _connection.StopAsync();
        
        if (data.Status == QuickAuthStatus.Declined)
        {
            OnAttemptDeclined?.Invoke();
            return;
        }
        
        var response = await _apiRepository.FinalizeQuickAuthSessionAsync(data);
        if (!response.IsSuccess)
        {
            OnAttemptDeclined?.Invoke();
            return;
        }
        
        OnAttemptSuccess?.Invoke(this, response);
    }
}