using System.Text.Json.Serialization;
using Backend.Domain.Configs;

namespace Backend.Domain.JsonContexts;

[JsonSourceGenerationOptions(WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.Never)]
[JsonSerializable(typeof(ApplicationConfig))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(VpnConfig))]
[JsonSerializable(typeof(VpnConfig.InputsObject))]
[JsonSerializable(typeof(VpnConfig.InputsObject.InputObj))]
[JsonSerializable(typeof(SplitTunnelingConfig))]
[JsonSerializable(typeof(GeneralSettingsConfig))]
[JsonSerializable(typeof(NetworkConfig))]
[JsonSerializable(typeof(DeveloperConfig))]
[JsonSerializable(typeof(AllInConfigStructure))]
[JsonSerializable(typeof(SplitTunnelingRuleType))]
[JsonSerializable(typeof(SplitTunnelingRule))]
public partial class ConfigsJsonContext : JsonSerializerContext;