namespace CShroudApp.Desktop.ViewModels.MainPages;

public partial class ServersViewModel : MainPageViewModelBasic
{
    public override MainPagesType MainPageType => MainPagesType.Servers;
    public override string Title => "Servers";
    public override string Description => "Select your DAW server";
}