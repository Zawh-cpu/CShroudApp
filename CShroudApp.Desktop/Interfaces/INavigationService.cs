using CShroudApp.Desktop.ViewModels;

namespace CShroudApp.Desktop.Interfaces;

public interface INavigationService
{
    event EventHandler<ViewModelBase>? ViewModelChanged;
    TViewModel GoTo<TViewModel>(params object[] args) where TViewModel : ViewModelBase;
    bool Back();
}