using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using Avalonia.Threading;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CShroudApp.Desktop.Services;
using CShroudApp.Desktop.ViewModels;

namespace CShroudApp.Desktop.Resources.Panels.Auth.ViewModels;

public enum SessionState
{
    Awaiting,
    Success,
    Failed
}

public partial class TelegramQuickLoginPanelViewModel : ViewModelBase, IDisposable
{
    public event Action? GoToDefaultLoginEvent;
    
    [ObservableProperty]
    private QuickAuthSessionDto? _quickSession;
    
    [ObservableProperty] private bool _isAvailableToNextTelegramQuickAuthSession = true;
    private DateTime _lastTelegramQuickAuthSessionRequest = DateTime.MinValue;
    
    private readonly DispatcherTimer _timer;
    
    private readonly string _countdownRetryButtonTextPattern = LocalizationHelper.GetTranslation("UiAuthRetryLoginViaTelegramButtonCountingDown", CultureInfo.CurrentCulture);
    private readonly string _defaultRetryButtonTextPattern = LocalizationHelper.GetTranslation("UiAuthRetryLoginViaTelegramButton", CultureInfo.CurrentCulture);

    [ObservableProperty] private string _retryButtonText;

    // private readonly IApiRepository _apiRepository;
    private readonly IQuickAuthService _quickAuthService;

    [ObservableProperty]
    private SessionState _quickAuthState = SessionState.Awaiting;
    
    private CancellationTokenSource _cancellationTokenSource = new();
    
    public RelayCommand OpenTelegramCommand { get; }
    
    public TelegramQuickLoginPanelViewModel(IQuickAuthService quickAuthService)
    {
        _quickAuthService = quickAuthService;
        _retryButtonText = _defaultRetryButtonTextPattern;
        
        _quickAuthService.OnSessionFailed += OnSessionFailed;
        _quickAuthService.OnAttemptSuccess += OnAttemptSuccess;
        _quickAuthService.OnAttemptDeclined += OnAttemptDeclined;
        
        _timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Default, CooldownForRetryTick);
        _timer.Stop();
        
        OpenTelegramCommand = new RelayCommand(() =>
        {
            if (_quickSession is not null)
                // OpenTelegram(_quickSession.Variants, _quickSession.SessionId.ToString());
                OpenTelegram(_quickSession.SessionId.ToString());
        });
    }
    
    [RelayCommand]
    private void GoToDefaultLogin()
    {
        GoToDefaultLoginEvent?.Invoke();
    }
    
    private void OpenTelegram(string fastLoginId)
    {
        var data = $"verify_{fastLoginId}";
        
        var url = $"https://t.me/VeryRichBitchBot?start={Convert.ToBase64String(Encoding.UTF8.GetBytes(data))}";
        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            process.Start();
        }
        catch (Exception)
        {
            // PASS
        }
    }

    private void OnSessionFailed()
    {
        QuickAuthState = SessionState.Failed;
    }

    private void OnAttemptDeclined()
    {
        QuickAuthState = SessionState.Failed;
    }

    private void OnAttemptSuccess(object? sender, SignInDto session)
    {
        QuickAuthState = SessionState.Success;
    }
    
    public void SetupQuickAuthSession(QuickAuthSessionDto session, DateTime lastTelegramQuickAuthSessionRequestObtained, CancellationTokenSource cts)
    {
        QuickSession = session;
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = cts;
        IsAvailableToNextTelegramQuickAuthSession = false;
        _lastTelegramQuickAuthSessionRequest = lastTelegramQuickAuthSessionRequestObtained;
        CooldownForRetryTick(null, EventArgs.Empty);
        _timer.Start();
        
        QuickAuthState = SessionState.Awaiting;
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
        IsAvailableToNextTelegramQuickAuthSession = false;
        
        await _cancellationTokenSource.CancelAsync();
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenSource.Token);
        
        var session = await _quickAuthService.RunSession(_cancellationTokenSource.Token);
        if (session.IsSuccess)
        {
            QuickSession = session;
            QuickAuthState = SessionState.Awaiting;
        }
        
        _timer.Start();
        CooldownForRetryTick(null, EventArgs.Empty);
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
        _cancellationTokenSource.Cancel();
    }
}