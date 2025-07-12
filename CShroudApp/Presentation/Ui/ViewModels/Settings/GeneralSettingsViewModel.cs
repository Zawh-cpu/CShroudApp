using CommunityToolkit.Mvvm.Input;
using CShroudApp.Presentation.Ui.Interfaces;

namespace CShroudApp.Presentation.Ui.ViewModels.Settings;

public partial class GeneralSettingsViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public GeneralSettingsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    public void Back()
    {
        if (!_navigationService.Back())
            _navigationService.GoTo<DashboardViewModel>();
    }
}