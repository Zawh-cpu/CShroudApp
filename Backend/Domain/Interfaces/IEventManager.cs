using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces;

public interface IEventManager
{
    public event Action<SignInDto>? OnSignInDataReceived;
    public void SignInDataReceived(SignInDto data);
    
    public event Action? OnSessionAuthenticated;
    public void SessionAuthenticated();
}