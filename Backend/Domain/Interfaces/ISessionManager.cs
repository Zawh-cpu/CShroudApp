using Ardalis.Result;
using Backend.Domain.Entities.User;

namespace Backend.Domain.Interfaces;

public interface ISessionManager
{
    public User Session { get; }
    
    public DateTime SessionExpires { get; }
    public Task<Result<User>> UpdateSession();
    
    public string? RefreshToken { get; set; }
    public string? ActionToken { set; }
}