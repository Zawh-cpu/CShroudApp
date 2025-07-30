using System.Globalization;
using Avalonia.Threading;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Desktop.Services;
using CShroudApp.Desktop.ViewModels;

namespace CShroudApp.Desktop.Resources.Panels.Auth.ViewModels;

public partial class TelegramQuickLoginPanelViewModel : ViewModelBase, IDisposable
{
    public event Action? GoToDefaultLoginEvent;
    
    [ObservableProperty]
    private QuickAuthSessionDto? _quickSession;
    
    [ObservableProperty] private bool _isAvailableToNextTelegramQuickAuthSession = true;
    private DateTime _lastTelegramQuickAuthSessionRequest = DateTime.MinValue;
    
    private DispatcherTimer _timer;
    
    private readonly string _countdownRetryButtonTextPattern = LocalizationHelper.GetTranslation("UiAuthRetryLoginViaTelegramButtonCountingDown", CultureInfo.CurrentCulture);
    private readonly string _defaultRetryButtonTextPattern = LocalizationHelper.GetTranslation("UiAuthRetryLoginViaTelegramButton", CultureInfo.CurrentCulture);

    [ObservableProperty] private string _retryButtonText;

    private readonly IApiRepository _apiRepository;
    
    public TelegramQuickLoginPanelViewModel(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
        _retryButtonText = _defaultRetryButtonTextPattern;
        
        _timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Default, CooldownForRetryTick);
        _timer.Stop();
    }
    
    [RelayCommand]
    private void GoToDefaultLogin()
    {
        GoToDefaultLoginEvent?.Invoke();
    }
    
    public void SetupQuickAuthSession(QuickAuthSessionDto dto, DateTime lastTelegramQuickAuthSessionRequestObtained)
    {
        QuickSession = dto;
        IsAvailableToNextTelegramQuickAuthSession = false;
        _lastTelegramQuickAuthSessionRequest = lastTelegramQuickAuthSessionRequestObtained;
        CooldownForRetryTick(null, EventArgs.Empty);
        _timer.Start();
    }

    private void CooldownForRetryTick(object? sender, EventArgs e)
    {
        var timeLeft = _lastTelegramQuickAuthSessionRequest - DateTime.UtcNow.AddSeconds(-60);
        if (timeLeft.TotalSeconds < 0)
        {
            RetryButtonText = _defaultRetryButtonTextPattern;
            IsAvailableToNextTelegramQuickAuthSession = true;
            _timer.Stop();
            return;
        }
        
        RetryButtonText = string.Format(_countdownRetryButtonTextPattern, timeLeft.Seconds);
    }
    
    [RelayCommand]
    private async Task RetryTelegramQuickAuthSession()
    {
        if (!IsAvailableToNextTelegramQuickAuthSession) return;
        _lastTelegramQuickAuthSessionRequest = DateTime.UtcNow;
        
        var session = await _apiRepository.BeginQuickAuthSessionAsync();
        IsAvailableToNextTelegramQuickAuthSession = false;
        CooldownForRetryTick(null, EventArgs.Empty);
        _timer.Start();
        if (!session.IsSuccess)
        {
            Console.WriteLine("Failed to initialize quick auth session (Telegram)");
            return;
        }

        QuickSession = session.Value;
    }

    public void Dispose()
    {
        _timer.Stop();
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        
        var timeLeft = _lastTelegramQuickAuthSessionRequest - DateTime.UtcNow.AddSeconds(-60);
        if (!IsAvailableToNextTelegramQuickAuthSession)
        {
            if (timeLeft.TotalSeconds >= 0)
            {
                IsAvailableToNextTelegramQuickAuthSession = false;
                _timer.Start();
            }
            
            CooldownForRetryTick(null, EventArgs.Empty);
        }
        
    }

    public override void OnUnloaded()
    {
        base.OnUnloaded();
        _timer.Stop();
    }
}