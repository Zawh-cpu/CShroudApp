using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CShroudApp.Desktop.Interfaces;

namespace CShroudApp.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView = null!;
    
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        _navigationService.ViewModelChanged += ChangeWindow;

        _navigationService.GoTo<LoginViewModel>();
    }
    
    public void ChangeWindow(object? sender, ViewModelBase view)
    {
        CurrentView?.OnUnloaded();
        CurrentView = view;
        CurrentView?.OnLoaded();
    }
}