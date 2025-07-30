using System;

namespace Backend.Application.DTOs;

public class SignInDto
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
    public required string ActionToken { get; init; }
}