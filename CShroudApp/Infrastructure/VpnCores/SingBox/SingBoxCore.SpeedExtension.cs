using System.Net.Http;
using System.Text.Json;
using System.Threading;
using CShroudApp.Core.Entities;
using CShroudApp.Infrastructure.VpnCores.SingBox.DTOs;
using CShroudApp.Infrastructure.VpnCores.SingBox.JsonContexts;

namespace CShroudApp.Infrastructure.VpnCores.SingBox;

public partial class SingBoxCore
{
    public ulong Upload { get; set; }
    public ulong Download { get; set; }

    public event Action<ulong, ulong>? SpeedUpdated;
    
    public CancellationTokenSource SpeedExtCts { get; set; }
    

    public async Task StartSpeedStreamAsync(CancellationToken token)
    {
        Console.WriteLine($"STARTING SPEED STREAM {_settings.DeveloperSettings.ClashApiPort}");
        try
        {
            var handler = new SocketsHttpHandler
            {
                UseProxy = false
            };
            
            HttpClient client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"http://127.0.0.1:{_settings.DeveloperSettings.ClashApiPort}/")
            };
            
            Console.WriteLine(client.BaseAddress);

            using var response = await client.GetAsync("traffic", HttpCompletionOption.ResponseHeadersRead, token);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(token);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !token.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var data = JsonSerializer.Deserialize<SpeedDto>(line, SingBoxJsonContext.Default.SpeedDto);
                    if (data == null) continue;

                    Upload = data.Up;
                    Download = data.Down;
                    
                    SpeedUpdated?.Invoke(Upload, Download);
                }
                catch (Exception)
                {
                    // Ignore error
                }
            }
        }
        catch (Exception)
        {
            // Ignore error
        }
        
        Upload = 0;
        Download = 0;
        SpeedUpdated?.Invoke(Upload, Download);
    }
}