using System.Drawing;
using Backend.Domain.Entities;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Backend.Domain.Interfaces;

public interface IInstalledAppsManager
{
    public List<ApplicationData> InstalledApps { get; }
    public Bitmap AvaloniaIconSystemIcon(Icon icon);
    public Icon GetDefaultIconForThisExtension(string extension);
}