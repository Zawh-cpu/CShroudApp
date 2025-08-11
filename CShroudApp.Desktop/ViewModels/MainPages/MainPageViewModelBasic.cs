namespace CShroudApp.Desktop.ViewModels.MainPages;

public abstract class MainPageViewModelBasic : ViewModelBase
{
    public abstract MainPagesType MainPageType { get; }
    
    public abstract string Title { get; }
    public abstract string Description { get; }
}

public enum MainPagesType
{
    Dashboard,
    Servers,
    Settings
}