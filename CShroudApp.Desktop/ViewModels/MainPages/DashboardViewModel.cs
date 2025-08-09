using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CShroudApp.Desktop.ViewModels.MainPages;


public partial class DashboardViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType => MainPagesType.Dashboard;
    public override string Title => "Dashboard";
    public override string Description => "Control your VPN connection and view status";
    
    public MainSharedMemory SharedMemory { get; set; }

    public DashboardViewModel(MainSharedMemory sharedMemory)
    {
        SharedMemory = sharedMemory;
    }
}