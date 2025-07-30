using System.Threading;
using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces;

public interface IQuickAuthService
{
    public event Action? OnAttemptDeclined;
    public event EventHandler<SignInDto>? OnAttemptSuccess;
    public event EventHandler<QuickAuthSessionDto>? OnSessionCreated;
    public event Action? OnSessionFailed;
    
    Task RunSession(CancellationToken cancellationToken);
}