using System.Text.Json.Serialization;
using Backend.Domain.Configs;

namespace Backend.Domain.JsonContexts;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
[JsonSerializable(typeof(ApplicationConfig))]
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class ConfigsJsonContext : JsonSerializerContext;