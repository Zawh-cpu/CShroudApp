using System.Collections.Generic;

namespace Backend.Domain.Configs;

public class NetworkConfig
{
    public List<string> ReservedGatewayAddresses { get; set; } = [ "http://localhost:5234" ];
}