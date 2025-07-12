using System.Collections.ObjectModel;
using CShroudApp.Core.Entities;

namespace CShroudApp.Presentation.Ui.ViewModels;

public class DesignerViewModel : ViewModelBase
{
    public ObservableCollection<NotificationObject> Notifications { get; } = new()
    {
        new NotificationObject()
        {
            Title = "VPN started",
            Message = "You can now use VPN",
            Type = NotificationType.Success
        }
    };

    public ObservableCollection<Location> AvailableLocations { get; } = new() { new Location
    {
        City = "frankfurt",
        Country = "de"
    } };
    
    public DesignerViewModel()
    {
        Console.WriteLine("------ DESIGNER VIEW MODEL HAS BEEN USED ------");
    }
}