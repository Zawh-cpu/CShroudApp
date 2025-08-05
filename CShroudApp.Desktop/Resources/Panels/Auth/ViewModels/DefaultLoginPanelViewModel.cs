using System.Threading;
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
    
    public event Action<QuickAuthSessionDto, DateTime, CancellationTokenSource>? GoToTelegramEvent;

    // private readonly IApiRepository _apiRepository;
    private readonly IQuickAuthService _quickAuthService;

    public DefaultLoginPanelViewModel(IQuickAuthService quickAuthService)
    {
        // _apiRepository = apiRepository;
        _quickAuthService = quickAuthService;
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

        var sourceToken = new CancellationTokenSource();
        var session = await _quickAuthService.RunSession(sourceToken.Token);
        SetupTelegramQuickAuthButtonState(true);
        if (!session.IsSuccess)
            return;
        
        Console.WriteLine(session.Value.SessionId);
        Console.WriteLine(session.Value.ValidVariant);
        GoToTelegramEvent?.Invoke(session.Value, _lastTelegramQuickAuthSessionRequest, sourceToken);
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