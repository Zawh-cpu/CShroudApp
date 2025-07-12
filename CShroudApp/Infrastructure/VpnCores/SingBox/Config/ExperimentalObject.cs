namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

public class ExperimentalObject
{
    public ExperimentalTempObject? CacheFile { get; set; }
    public ClashApiObject? ClashApi { get; set; }

    public class ExperimentalTempObject
    {
        public bool Enabled { get; set; } = true;
        public string? Path { get; set; }
    }

    public class ClashApiObject
    {
        public string? ExternalController { get; set; }
        public string? ExternalUi { get; set; }
        public string? ExternalUiDownloadUrl { get; set; }
        public string? ExternalUiDownloadDetour { get; set; }
        public string? Secret { get; set; }
        public string? DefaultMode { get; set; }
        public string[]? AccessControlAllowOrigin { get; set; }
        public bool AccessControlAllowPrivateNetwork { get; set; }
    }
}