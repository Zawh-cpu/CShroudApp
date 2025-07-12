using System.IO;
using Ardalis.Result;

namespace CShroudApp.Core.Interfaces;

public interface IProcess
{
    Result Start(bool reactivateProcess = true);
    void Kill();

    Task KillAsync();
    
    bool IsRunning { get; }
    bool HasExited { get; }

    public StreamWriter StandardInput { get; }
    
    event EventHandler ProcessExited;
    event EventHandler ProcessStarted;
}