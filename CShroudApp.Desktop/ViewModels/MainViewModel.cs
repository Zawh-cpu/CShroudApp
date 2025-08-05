using Backend.Domain.Entities.User;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CShroudApp.Desktop.ViewModels.MainPages;
using Microsoft.Extensions.DependencyInjection;

namespace CShroudApp.Desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static Dictionary<MainPagesType, Type> MainPagesTypes { get; } = new()
    {
        [MainPagesType.Dashboard] = typeof(DashboardViewModel),
        [MainPagesType.Servers] = typeof(ServersViewModel)
    };

    private readonly IServiceProvider _serviceProvider;
    private readonly ISessionManager _sessionManager;
    private readonly IEventManager _eventManager;
    
    [ObservableProperty]
    private MainPageViewModelBasic _currentPage;
    
    public User Session => _sessionManager.Session;

    public MainViewModel(IServiceProvider serviceProvider, ISessionManager sessionManager, IEventManager eventManager)
    {
        _serviceProvider = serviceProvider;
        _sessionManager = sessionManager;
        _eventManager = eventManager;

        CurrentPage = new DashboardViewModel();
    }

    public void NavbarChangePageButtonClicked(MainPagesType mainPageType)
    {
        if (MainPagesTypes.TryGetValue(mainPageType, out var type))
            CurrentPage = (MainPageViewModelBasic)_serviceProvider.GetRequiredService(type);
    }
}