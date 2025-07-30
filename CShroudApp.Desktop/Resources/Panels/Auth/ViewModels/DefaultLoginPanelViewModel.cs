using Avalonia;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Desktop.ViewModels;

namespace CShroudApp.Desktop.Resources.Panels.Auth.ViewModels;

public partial class DefaultLoginPanelViewModel : ViewModelBase, IDisposable
{
    [ObservableProperty]
    private string _loginViaTelegramButtonKey = "UiAuthLoginViaTelegramButton";

    [ObservableProperty] private bool _isAvailableToNextTelegramQuickAuthSession = true;
    private DateTime _lastTelegramQuickAuthSessionRequest = DateTime.MinValue;
    
    public event Action<QuickAuthSessionDto, DateTime>? GoToTelegramEvent;

    private IApiRepository _apiRepository;

    public DefaultLoginPanelViewModel(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    public void SetupTelegramQuickAuthButtonState(bool isAvailable)
    {
        IsAvailableToNextTelegramQuickAuthSession = isAvailable;
        if (isAvailable)
        {
            LoginViaTelegramButtonKey = "UiAuthLoginViaTelegramButton";
        }
        else
        {
            LoginViaTelegramButtonKey = "UiAuthLoginViaTelegramButtonConnecting";
        }
    }

    [RelayCommand]
    private async Task InitializeTelegramQuickAuthSession()
    {
        if (!IsAvailableToNextTelegramQuickAuthSession) return;
        SetupTelegramQuickAuthButtonState(false);
        _lastTelegramQuickAuthSessionRequest = DateTime.UtcNow;
        
        var session = await _apiRepository.BeginQuickAuthSessionAsync();
        SetupTelegramQuickAuthButtonState(true);
        if (!session.IsSuccess)
        {
            Console.WriteLine("Failed to initialize quick auth session (Telegram)");
            return;
        }

        GoToTelegramEvent?.Invoke(session.Value, _lastTelegramQuickAuthSessionRequest);
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        
        if (_lastTelegramQuickAuthSessionRequest < DateTime.UtcNow.AddSeconds(30))
            SetupTelegramQuickAuthButtonState(true);
    }

    public void Dispose()
    {
        GoToTelegramEvent = null;
    }
}