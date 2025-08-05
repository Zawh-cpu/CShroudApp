using System.Net.Http;
using System.Threading;
using Ardalis.Result;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Backend.Infrastructure.Services;

public class QuickAuthService : IQuickAuthService
{
    private readonly HubConnection _connection;
    private readonly IApiRepository _apiRepository;
    private readonly IEventManager _eventManager;
    
    public event Action? OnAttemptDeclined;
    public event EventHandler<SignInDto>? OnAttemptSuccess;
    public event EventHandler<QuickAuthSessionDto>? OnSessionCreated;
    public event Action? OnSessionFailed;

    public QuickAuthService(IHttpClientFactory httpClientFactory, IApiRepository apiRepository, IEventManager eventManager)
    {
        _apiRepository = apiRepository;
        _eventManager = eventManager;
        
        _connection = new HubConnectionBuilder()
            .WithUrl(httpClientFactory.CreateClient("CrimsonShroudApiHook").BaseAddress + "api/v1/quick-auth-hub")
            .Build();
        
        _connection.On<QuickAuthDto>("OnStatusChanged", OnStatusChanged);
    }

    public async Task<Result<QuickAuthSessionDto>> RunSession(CancellationToken cancellationToken)
    {
        if (_connection.State != HubConnectionState.Disconnected)
            await _connection.StopAsync(cancellationToken);
        
        var session = await _apiRepository.BeginQuickAuthSessionAsync();
        if (!session.IsSuccess)
        {
            OnSessionFailed?.Invoke();
            return session.Map();
        }
        
        OnSessionCreated?.Invoke(this, session);
        
        await _connection.StartAsync(cancellationToken);
        await _connection.InvokeAsync("SubscribeToSession", session.Value.SessionId, cancellationToken: cancellationToken);

        return session;
    }

    private async Task OnStatusChanged(QuickAuthDto data)
    {
        if (_connection.State != HubConnectionState.Disconnected)
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
        _eventManager.SignInDataReceived(response);
    }
}