using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using Avalonia;
using Avalonia.Collections;
using Backend;
using Backend.Domain.Configs;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace CShroudApp.Desktop.ViewModels.MainPages;

public struct ApplicationDataDto
{
    public string Name { get; set; }
    public string Value { get; set; }
    public SplitTunnelingRuleType Type { get; set; }
    public bool IsCustom { get; set; }
    
    public Bitmap Icon { get; set; }
}

public partial class SettingsViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType { get; } = MainPagesType.Settings;
    public override string Title { get; } = "Settings";
    public override string Description { get; } = "Configure your preferences and security";

    //private static readonly Bitmap DefaultIcon = new("avares://CShroudApp.Desktop/Assets/Icons/png/default-app.png");

    public MainSharedMemory SharedMemory { get; set; }
    private IInstalledAppsManager InstalledAppsManager { get; }
    

    public ObservableCollection<ApplicationDataDto> AvailableRulesList { get; set; } = [];
    public ObservableCollection<ApplicationDataDto> FilteredAvailableRulesList => AvailableRulesList;
    public ObservableCollection<ApplicationDataDto> SelectedSplitTunnelingRules { get; set; } = [];
    
    public SettingsViewModel(MainSharedMemory sharedMemory, IInstalledAppsManager installedAppsManager)
    {
        SharedMemory = sharedMemory;
        InstalledAppsManager = installedAppsManager;

        SelectedSplitTunnelingRules.CollectionChanged += OnSelectedSplitTunnelingRulesChanged;
    }
    
    public SettingsViewModel()
    {
        var host = BackendStarter.Start([], App.GetUiDependencyCollection());

        MainSharedMemory sharedMemory = host.Services.GetRequiredService<MainSharedMemory>();
        IInstalledAppsManager installedAppsManager = host.Services.GetRequiredService<IInstalledAppsManager>();
        
        SharedMemory = sharedMemory;
        InstalledAppsManager = installedAppsManager;
        
        SelectedSplitTunnelingRules.CollectionChanged += OnSelectedSplitTunnelingRulesChanged;
    }

    [RelayCommand]
    public void CheckForUpdates()
    {
        Console.WriteLine(SharedMemory.UiCachedOptions.SettingsIsSplitTunnelingAppsSelectorCollapsed);
    }

    [RelayCommand]
    private void OpenSplitTunnelingList()
    {
        if (AvailableRulesList.Count != 0) return;
        
        var temp = new Dictionary<string, ApplicationDataDto>();

        foreach (var app in InstalledAppsManager.InstalledApps)
        {
            var a = new ApplicationDataDto()
            {
                Name = app.Name,
                Value = app.ExecutablePath,
                Icon = InstalledAppsManager.AvaloniaIconSystemIcon(app.Icon ??
                                                                   InstalledAppsManager.GetDefaultIconForThisExtension(
                                                                       Path.GetExtension(app.ExecutablePath))),
                Type = SplitTunnelingRuleType.Path,
                IsCustom = false
            };
            
            AvailableRulesList.Add(a);
            temp[app.ExecutablePath] = a;
        }

        foreach (var rule in SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules)
        {
            ApplicationDataDto dto;
            if (!temp.TryGetValue(rule.Value, out dto))
            {
                dto = new ApplicationDataDto()
                {
                    Name = rule.Name,
                    Value = rule.Value,
                    Icon = InstalledAppsManager.AvaloniaIconSystemIcon(InstalledAppsManager.GetDefaultIconForThisExtension(Path.GetExtension(rule.Value))),
                    Type = rule.Type,
                    IsCustom = rule.IsCustom
                };
                
                AvailableRulesList.Add(dto);
            }
            
            if (rule.Enabled)
                SelectedSplitTunnelingRules.Add(dto);
        }
    }

    private void OnSelectedSplitTunnelingRulesChanged(object? obj, NotifyCollectionChangedEventArgs args)
    {
        if (args.Action == NotifyCollectionChangedAction.Reset)
        {
            foreach (var rule in SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.ToArray())
            {
                if (rule.IsCustom)
                {
                    rule.Enabled = false;
                    continue;
                }
                
                SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.Remove(rule);
            }
            return;
        }
        
        if (args.NewItems != null)
            foreach (var item in args.NewItems)
            {
                if (item is not ApplicationDataDto dto) continue;

                SplitTunnelingRule? temp;
                
                temp = SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.FirstOrDefault(x => x.Value == dto.Value);
                if (temp is not null)
                {
                    temp.Enabled = true;
                    continue;
                }
                
                SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.Add(new SplitTunnelingRule()
                {
                    Enabled = true,
                    Name = dto.Name,
                    Value = dto.Value,
                    IsCustom = dto.IsCustom,
                    Type = dto.Type
                });
            }
        
        if (args.OldItems != null)
            foreach (var item in args.OldItems)
            {
                if (item is not ApplicationDataDto dto) continue;
                if (dto.IsCustom)
                {
                    var temp = SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.FirstOrDefault(x => x.Value == dto.Value && x.IsCustom);
                    if (temp is not null)
                        temp.Enabled = false;
                    continue;
                }

                foreach (var rule in SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.Where(x =>
                             x.Value == dto.Value).ToArray())
                    SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Rules.Remove(rule);
            }
    }

    [RelayCommand]
    private void SplitTunnelingUncheckAll()
    {
        SelectedSplitTunnelingRules.Clear();
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        if (SharedMemory.ApplicationConfig.Vpn.SplitTunneling.Enabled)
            OpenSplitTunnelingList();
    }

    public override void OnUnloaded()
    {
        base.OnUnloaded();
        AvailableRulesList.Clear();
        SelectedSplitTunnelingRules.Clear();
    }
}