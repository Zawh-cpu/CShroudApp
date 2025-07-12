﻿using System.Collections.Generic;

namespace CShroudApp.Core.Configs;

public class NetworkConfig
{
    public List<string> ReservedGatewayAddresses { get; set; } = [ "http://localhost:5234" ];
}