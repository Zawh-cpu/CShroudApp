using System.Text.Json.Serialization;
using Backend.Infrastructure.VpnCores.SingBox.Config;
using Backend.Infrastructure.VpnCores.SingBox.DTOs;
using Backend.Infrastructure.VpnCores.SingBox.Config.Bounds;
using Backend.Infrastructure.VpnCores.SingBox.Mappers;

namespace Backend.Infrastructure.VpnCores.SingBox.JsonContexts;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)]
[JsonSerializable(typeof(TopConfig))]
[JsonSerializable(typeof(SpeedDto))]
public partial class SingBoxJsonContext : JsonSerializerContext;