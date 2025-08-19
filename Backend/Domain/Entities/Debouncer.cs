using System.Threading;

namespace Backend.Domain.Entities;

public class Debouncer
{
    private CancellationTokenSource? _cts;

    public void Debounce(Action action, int milliseconds)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        var token = _cts.Token;

        Task.Delay(milliseconds, token)
            .ContinueWith(t =>
            {
                if (!t.IsCanceled)
                    action();
            }, TaskScheduler.Default);
    }
    
    public void Debounce(Func<Task> action, int milliseconds)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(milliseconds, token);
                if (!token.IsCancellationRequested)
                {
                    await action();
                }
            }
            catch (TaskCanceledException)
            {
                // ignore
            }
        });
    }
}