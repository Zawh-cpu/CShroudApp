using System.Drawing;
using System.Runtime.InteropServices;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Microsoft.Win32;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace Backend.Infrastructure.Platforms.Windows.Services;

public class WindowsInstalledAppsManager : IInstalledAppsManager
{
    private static readonly Dictionary<string, Icon> IconCache = new(StringComparer.OrdinalIgnoreCase);

    private static List<ApplicationData> GetInstalledApps()
    {
        List<ApplicationData> apps = new(64);
        
        string[] registryKeys = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };

        foreach (var keyPath in registryKeys)
        {
            using var key = Registry.LocalMachine.OpenSubKey(keyPath);
            if (key == null) continue;

            foreach (var subkeyName in key.GetSubKeyNames())
            {
                using var subkey = key.OpenSubKey(subkeyName);
                var name = subkey?.GetValue("DisplayName") as string;
                var exePath = subkey?.GetValue("DisplayIcon") as string;

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(exePath))
                {
                    exePath = exePath.Trim('"');
                    int commaIndex = exePath.IndexOf(',');
                    if (commaIndex >= 0)
                        exePath = exePath.Substring(0, commaIndex);
                    Icon? icon = null;
                    if (File.Exists(exePath))
                    {
                        try { icon = Icon.ExtractAssociatedIcon(exePath); } catch { }
                    }
                    
                    apps.Add(new ApplicationData() { Name = name, Icon = icon, ExecutablePath = exePath});
                }
            }
        }

        return apps;
    }
    
    public Bitmap AvaloniaIconSystemIcon(Icon icon)
    {
        using var ms = new MemoryStream();
        icon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        ms.Seek(0, SeekOrigin.Begin);
        return new Bitmap(ms);
    }
    
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SHGetFileInfo(
        string pszPath,
        uint dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbFileInfo,
        uint uFlags
    );

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    private const uint SHGFI_ICON = 0x000000100;
    private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
    private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

    public Icon GetDefaultIconForThisExtension(string extension)
    {
        if (IconCache.TryGetValue(extension, out var cachedIcon))
            return cachedIcon;
        
        /*var shfi = new SHFILEINFO();
        SHGetFileInfo(extension, FILE_ATTRIBUTE_NORMAL, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_USEFILEATTRIBUTES);
        return Icon.FromHandle(shfi.hIcon);*/
        
        SHFILEINFO shinfo = new SHFILEINFO();
        SHGetFileInfo(extension, FILE_ATTRIBUTE_NORMAL, ref shinfo, (uint)Marshal.SizeOf(shinfo),
            SHGFI_ICON | SHGFI_USEFILEATTRIBUTES);
        
        if (shinfo.hIcon == IntPtr.Zero)
            return null;
        
        var icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();

        IconCache[extension] = icon;

        return icon;
    }
    
    public List<ApplicationData> InstalledApps { get; } = GetInstalledApps();
}