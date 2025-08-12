using System.Collections.ObjectModel;
using System.Drawing;
using Avalonia.Collections;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.Input;

using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace CShroudApp.Desktop.ViewModels.MainPages;

public struct ApplicationDataDto
{
    public string Name { get; set; }
    public string ExecutablePath { get; set; }
    public Bitmap? Icon { get; set; }
    public bool IsIncluded { get; set; }
}

public partial class SettingsViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType { get; } = MainPagesType.Settings;
    public override string Title { get; } = "Settings";
    public override string Description { get; } = "Configure your preferences and security";

    private static readonly Bitmap DefaultIcon = new("avares://CShroudApp.Desktop/Assets/Icons/png/default-app.png");

    public MainSharedMemory SharedMemory { get; set; }
    private IInstalledAppsManager InstalledAppsManager { get; }

    public ObservableCollection<ApplicationDataDto> ApplicationList { get; set; }

    public SettingsViewModel(MainSharedMemory sharedMemory, IInstalledAppsManager installedAppsManager)
    {
        SharedMemory = sharedMemory;
        InstalledAppsManager = installedAppsManager;

        ApplicationList = new(InstalledAppsManager.InstalledApps.Select(x => new ApplicationDataDto()
        {
            Name = x.Name,
            ExecutablePath = x.ExecutablePath,
            Icon = InstalledAppsManager.AvaloniaIconSystemIcon(x.Icon ?? InstalledAppsManager.GetDefaultIconForThisExtension(Path.GetExtension(x.ExecutablePath))),
            IsIncluded = false
        }));
    }

    [RelayCommand]
    public void CheckForUpdates()
    {
        Console.WriteLine(SharedMemory.ApplicationConfig.GeneralSettings.StartOnSystemStartup);
    }

    [RelayCommand]
    public void UpdateConfig()
    {
        Console.WriteLine("FWEFWEFWEFWEFWEF");
        Console.WriteLine(SharedMemory.ApplicationConfig.GeneralSettings.StartOnSystemStartup);
    }
}