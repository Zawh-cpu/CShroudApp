using System.Text.Json.Serialization;
using CShroudApp.Application.DTOs;
using CShroudApp.Core.Entities;

namespace CShroudApp.Core.JsonContexts;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(SignInDto))]
[JsonSerializable(typeof(SignInDataDto))]
[JsonSerializable(typeof(QuickAuthSessionDto))]
[JsonSerializable(typeof(ActionTokenRefreshDto))]
[JsonSerializable(typeof(GetUserDto))]
[JsonSerializable(typeof(Location[]))]
public partial class DtoJsonContext : JsonSerializerContext;