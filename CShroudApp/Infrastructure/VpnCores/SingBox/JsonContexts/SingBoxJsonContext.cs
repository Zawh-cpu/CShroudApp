using System.Text.Json.Serialization;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config;
using CShroudApp.Infrastructure.VpnCores.SingBox.Config.Bounds;
using CShroudApp.Infrastructure.VpnCores.SingBox.DTOs;
using CShroudApp.Infrastructure.VpnCores.SingBox.Mappers;

namespace CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)]
[JsonSerializable(typeof(TopConfig))]
[JsonSerializable(typeof(SpeedDto))]
public partial class SingBoxJsonContext : JsonSerializerContext;