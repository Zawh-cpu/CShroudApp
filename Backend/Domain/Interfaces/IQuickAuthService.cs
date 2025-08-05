using System.Threading;
using Ardalis.Result;
using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces;

public interface IQuickAuthService
{
    public event Action? OnAttemptDeclined;
    public event EventHandler<SignInDto>? OnAttemptSuccess;
    public event Action? OnSessionFailed;
    
    Task<Result<QuickAuthSessionDto>> RunSession(CancellationToken cancellationToken);
}