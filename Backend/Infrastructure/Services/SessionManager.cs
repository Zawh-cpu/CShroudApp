using Ardalis.Result;
using Backend.Application.DTOs;
using Backend.Domain.Entities;
using Backend.Domain.Entities.User;
using Backend.Domain.Interfaces;

namespace Backend.Infrastructure.Services;

public class SessionManager : ISessionManager
{
    private readonly IApiRepository _apiRepository;
    private readonly IStorageManager _storageManager;
    private readonly IEventManager _eventManager;
    
    private User _session = User.Unauthenticated();
    public DateTime SessionExpires { get; private set; } = DateTime.MinValue;
    
    public User Session
    {
        get
        {
            if (SessionExpires <= DateTime.Now)
            {
                var result = Task.Run(UpdateSession).Result;
                if (result.IsSuccess)
                {
                    _session = result.Value;
                    SessionExpires = DateTime.Now.AddMinutes(60);
                }
                else
                {
                    // TODO: Check for unauthorized actions
                    //UnauthorizedSession?.Invoke(this, EventArgs.Empty);
                    return User.Unauthenticated();
                }
            }
            
            return _session;
        }
        
        set => _session = value;
    }
    
    public string? RefreshToken
    {
        get => _apiRepository.RefreshToken;
        set
        {
            if (value is not null)
            {
                _storageManager.RefreshToken = value;
                _apiRepository.RefreshToken = value;
            }
        }
    }

    public string? ActionToken
    {
        set => _apiRepository.ActionToken = value;
        get => _apiRepository.ActionToken;
    }

    public SessionManager(IApiRepository apiRepository, IStorageManager storageManager, IEventManager eventManager)
    {
        _apiRepository = apiRepository;
        _storageManager = storageManager;
        _eventManager = eventManager;
        
        var token = _storageManager.RefreshToken;
        if (token is not null)
        {
            var parsedToken = Token.Parse(token);
            if (parsedToken.Expiration > DateTime.UtcNow)
            {
                RefreshToken = token;
                _eventManager.SessionAuthenticated();
            }
        }

        _eventManager.OnSignInDataReceived += SignInDataReceived;
    }

    private void SignInDataReceived(SignInDto data)
    {
        RefreshToken = data.RefreshToken;
        ActionToken = data.ActionToken;
        
        _eventManager.SessionAuthenticated();
    }
    
    public async Task<Result<User>> UpdateSession()
    {
        var result = await _apiRepository.GetUserInformationAsync();
        if (!result.IsSuccess)
            return result.Map();
        
        return new User()
        {
            Id = result.Value.Id,
            IsVerified = result.Value.IsVerified,
            Nickname = result.Value.Nickname,
            Rate = result.Value.Rate,
            Role = result.Value.Role
        };
    }

    public void Logout()
    {
        //LogoutAction?.Invoke();
    }
}