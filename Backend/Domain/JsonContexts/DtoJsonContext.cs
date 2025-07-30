using System.Text.Json.Serialization;
using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Domain.JsonContexts;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(SignInDto))]
[JsonSerializable(typeof(SignInDataDto))]
[JsonSerializable(typeof(QuickAuthSessionDto))]
[JsonSerializable(typeof(ActionTokenRefreshDto))]
[JsonSerializable(typeof(GetUserDto))]
[JsonSerializable(typeof(Location[]))]
public partial class DtoJsonContext : JsonSerializerContext;