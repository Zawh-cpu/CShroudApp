using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CShroudApp.Desktop.Interfaces;

namespace CShroudApp.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;
    
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService, IEventManager eventManager, ISessionManager sessionManager)
    {
        _navigationService = navigationService;
        
        _navigationService.ViewModelChanged += ChangeWindow;

        if (sessionManager.RefreshToken is null)
            _navigationService.GoTo<LoginViewModel>();
        else
            _navigationService.GoTo<MainViewModel>();
        
        eventManager.OnSessionAuthenticated += () => _navigationService.GoTo<MainViewModel>();
    }
    
    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}