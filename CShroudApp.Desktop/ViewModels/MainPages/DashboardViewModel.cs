namespace CShroudApp.Desktop.ViewModels.MainPages;

public partial class DashboardViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType => MainPagesType.Dashboard;
    public override string Title => "Dashboard";
    public override string Description => "Control your VPN connection and view status";
}