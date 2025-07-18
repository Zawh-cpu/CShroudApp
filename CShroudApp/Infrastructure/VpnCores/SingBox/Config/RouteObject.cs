﻿namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

public class RouteObject
{
    public List<RouteRule> Rules { get; set; } = new();
    public List<RouteRuleSet> RuleSet { get; set; } = new();
    public string? Final { get; set; }
    public bool? AutoDetectInterface { get; set; }
    public bool? OverrideAndroidVpn { get; set; }
    public string? DefaultInterface { get; set; }
    public int? DefaultMark { get; set; }
    public string? DefaultDomainResolver { get; set; }
    public string? DefaultNetworkStrategy { get; set; }
    public List<object>? DefaultNetworkType { get; set; }
    public List<object>? DefaultFallbackNetworkType { get; set; }
    public string? DefaultFallbackDelay { get; set; }

    public class RouteRule
    {
        public string? Outbound { get; set; }
        public string? Protocol { get; set; }
        public string[]? Domain { get; set; }
        public string[]? DomainSuffix { get; set; }
        public string[]? Network { get; set; }
        public string[]? RuleSet { get; set; }
        public string[]? IpCidr { get; set; }
        public string[]? ProcessName { get; set; }
        public string[]? ProcessPath { get; set; }
        
        public uint[]? Port { get; set; }
        public bool? IpIsPrivate { get; set; }
        public string? Action { get; set; }
        public string? ClashMode { get; set; }
    }
    
    public class RouteRuleSet
    {
        public required string Tag { get; set; }
        public required string Type { get; set; }
        public string? Format { get; set; }
        public string? Path { get; set; }
    }
}