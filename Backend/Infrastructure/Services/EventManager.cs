using Backend.Application.DTOs;
using Backend.Domain.Interfaces;

namespace Backend.Infrastructure.Services;

public class EventManager : IEventManager
{
    public event Action<SignInDto>? OnSignInDataReceived;
    
    public void SignInDataReceived(SignInDto data)
    {
        OnSignInDataReceived?.Invoke(data);
    }

    public event Action? OnSessionAuthenticated;
    public void SessionAuthenticated()
    {
        OnSessionAuthenticated?.Invoke();
    }

    public event Action? OnFailedConnectToNetwork;
    public void FailedConnectToNetwork()
    {
        OnFailedConnectToNetwork?.Invoke();
    }

    public event Action? OnConnectedToNetworkSuccessfully;
    public void ConnectedToNetworkSuccessfully()
    {
        OnConnectedToNetworkSuccessfully?.Invoke();
    }
}