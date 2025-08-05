using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CShroudApp.Desktop.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected bool IsShowedNow = false;
    
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = new();
    
    public virtual void OnLoaded()
    {
        CancellationTokenSource = new CancellationTokenSource();
        IsShowedNow = true;
    }

    public virtual void OnUnloaded()
    {
        CancellationTokenSource.Cancel();
        IsShowedNow = false;
    }
}