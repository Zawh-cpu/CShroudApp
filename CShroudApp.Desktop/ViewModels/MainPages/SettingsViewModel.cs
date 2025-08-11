namespace CShroudApp.Desktop.ViewModels.MainPages;

public class SettingsViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType { get; } = MainPagesType.Settings;
    public override string Title { get; } = "Settings";
    public override string Description { get; } = "Configure your preferences and security";
    
    
    public MainSharedMemory SharedMemory { get; set; }

    public SettingsViewModel(MainSharedMemory sharedMemory)
    {
        SharedMemory = sharedMemory;
    }
}