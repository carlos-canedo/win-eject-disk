using System.Diagnostics;
using WinEjectDisk.App.Constants;
using WinEjectDisk.App.Extensions;
using WinEjectDisk.App.Services;

internal sealed class TrayHost : IDisposable
{
    // FIXME: refactor constants
    private readonly NotifyIcon _tray;
    private readonly ContextMenuStrip _menu;

    public TrayHost()
    {
        _menu = new ContextMenuStrip();

        AddDiskToMenu(_menu);

        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add("Exit", null, OnExit);

        _tray = CreateTrayIcon();
    }

    private void AddDiskToMenu(ContextMenuStrip menu)
    {
        var externalDisks = DiskManagementService.GetDisks()
            .Where(d => d.IsExternal());

        foreach (var disk in externalDisks)
        {
            var item = new ToolStripMenuItem(disk.FriendlyName);
            if (disk.IsOffline)
            {
                item.DropDownItems.Add("Enable", null, (_, _) =>
                {
                    DiskManagementService.SetIsOffline(disk.Number, false);
                    Debug.WriteLine("Enable");
                });
            }
            else
            {
                item.DropDownItems.Add("Disable", null, (_, _) =>
                {
                    DiskManagementService.SetIsOffline(disk.Number, true);
                    Debug.WriteLine("Disable");
                });
            }

            menu.Items.Add(item);
        }
    }

    private NotifyIcon CreateTrayIcon()
    {
        string iconPath = Path.Combine(AppContext.BaseDirectory, Config.TrayIconPath);

        var trayIcon = new NotifyIcon
        {
            // FIXME: Refactor constants
            Icon = new Icon(iconPath),
            Text = "Eject External Disks",
            ContextMenuStrip = _menu,
            Visible = true
        };

        return trayIcon;
    }

    private void OnExit(object? sender, EventArgs e)
    {
        Dispose();
        Application.Exit();
    }

    public void Dispose()
    {
        _tray.Visible = false;
        _tray.Dispose();
        _menu.Dispose();
    }
}
