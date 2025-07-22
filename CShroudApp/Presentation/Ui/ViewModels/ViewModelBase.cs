using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CShroudApp.Presentation.Ui.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected bool _isShowedNow = false;
    
    protected CancellationTokenSource? CancellationTokenSource { get; private set; }
    
    public virtual void OnLoaded()
    {
        CancellationTokenSource = new CancellationTokenSource();
        
        _isShowedNow = true;
    }

    public virtual void OnUnloaded()
    {
        CancellationTokenSource?.Cancel();
        CancellationTokenSource?.Dispose();
        CancellationTokenSource = null;
        
        _isShowedNow = false;
    }
    
    public virtual void OnNavigated() {}
}