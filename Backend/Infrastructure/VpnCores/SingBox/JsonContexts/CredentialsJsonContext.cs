using System.Text.Json.Serialization;
using Backend.Infrastructure.VpnCores.SingBox.Mappers;

namespace Backend.Infrastructure.VpnCores.SingBox.JsonContexts;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)]
[JsonSerializable(typeof(VlessCredentials))]
public partial class CredentialsJsonContext : JsonSerializerContext;