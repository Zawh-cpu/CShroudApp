using CShroudApp.Presentation.Ui.Interfaces;
using CShroudApp.Presentation.Ui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Presentation.Ui.Services;

public class NavigationService : INavigationService
{
    private IServiceProvider _serviceProvider;
    
    private List<ViewModelBase> _navigationHistory = new(5);

    public event EventHandler<ViewModelBase>? ViewModelChanged;
    
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool Back()
    {
        if (_navigationHistory.Count < 2) return false;
        
        _navigationHistory.RemoveAt(_navigationHistory.Count - 1);
        var last = _navigationHistory.Last();

        last.OnNavigated();
        ViewModelChanged?.Invoke(this, last);
        
        return true;
    }
    
    public TViewModel GoTo<TViewModel>(params object[] args) where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        //viewModel.SwapData(args);
        viewModel.OnNavigated();
        ViewModelChanged?.Invoke(this, viewModel);
        
        if (_navigationHistory.Count >= 5)
            _navigationHistory.RemoveAt(0);
        _navigationHistory.Add(viewModel);
        
        return viewModel;
    }
}