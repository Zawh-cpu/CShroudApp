using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Desktop.Resources.Panels.Auth.ViewModels;

namespace CShroudApp.Desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase, IDisposable
{
    public DefaultLoginPanelViewModel DefaultLoginVm { get; }
    public TelegramQuickLoginPanelViewModel TelegramQuickLoginVm { get; }
    
    [ObservableProperty]
    private ViewModelBase _currentPanel;
    
    private readonly IApiRepository _apiRepository;
    
    public LoginViewModel(DefaultLoginPanelViewModel defaultLoginVm, TelegramQuickLoginPanelViewModel telegramQuickLoginVm, IApiRepository apiRepository)
    {
        DefaultLoginVm = defaultLoginVm;
        TelegramQuickLoginVm = telegramQuickLoginVm;
        _apiRepository = apiRepository;
        
        _currentPanel = DefaultLoginVm;
        DefaultLoginVm.GoToTelegramEvent += GoToTelegramQuickLogin;
        TelegramQuickLoginVm.GoToDefaultLoginEvent += GoToDefaultLogin;
    }
    
    private void GoToTelegramQuickLogin(QuickAuthSessionDto session, DateTime lastTelegramQuickAuthSessionRequestObtained)
    {
        TelegramQuickLoginVm.SetupQuickAuthSession(session, lastTelegramQuickAuthSessionRequestObtained);
        
        CurrentPanel.OnUnloaded();
        CurrentPanel = TelegramQuickLoginVm;
        CurrentPanel.OnLoaded();
    }
    
    private void GoToDefaultLogin()
    {
        CurrentPanel.OnUnloaded();
        CurrentPanel = DefaultLoginVm;
        CurrentPanel.OnLoaded();
    }

    public void Dispose()
    {
        DefaultLoginVm.GoToTelegramEvent -= GoToTelegramQuickLogin;
        TelegramQuickLoginVm.GoToDefaultLoginEvent -= GoToDefaultLogin;
    }
}