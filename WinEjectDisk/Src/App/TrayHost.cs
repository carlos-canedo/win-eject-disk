using System.Diagnostics;
using WinEjectDisk.Src.Core.Constants;
using WinEjectDisk.Src.Core.Domain.Entities;
using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.App;

internal sealed class TrayHost : IDisposable
{
    // FIXME: refactor constants
    private readonly DisksStateService _state;
    private readonly NotifyIcon _icon;
    private ContextMenuStrip? _menu;

    public TrayHost()
    {
        _state = new DisksStateService();
        _icon = CreateTrayIcon();

        _state.StateChanged += OnStateChanged;
        _icon.MouseClick += OnIconClicked;
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
        _icon.Visible = false;
        _icon.Dispose();
        _menu?.Dispose();
    }

    private void OnIconClicked(object? sender, MouseEventArgs e)
    {
        Logger.Log(e.Button.ToString());

        if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
        {
            _state.RefreshDisks();
        }
    }

    private void OnStateChanged(object? sender, DisksState state)
    {
        RebuildMenu(state);
    }

    private ToolStripMenuItem GetDiskMenuItem(Disk disk)
    {
        string itemText = $"{disk.FriendlyName} ({disk.FriendlySize})";
        var item = new ToolStripMenuItem(itemText);

        Logger.Log($"${itemText} - ${disk.DriveType}");

        if (disk.IsOffline)
        {
            // FIXME: these events should be handled in the app state service
            item.DropDownItems.Add("Enable", null, (_, _) =>
            {
                Debug.WriteLine("Enable");
                DiskManagementService.SetIsOffline(disk.Number, false);
            });

            return item;
        }

        item.DropDownItems.Add("Disable", null, (_, _) =>
        {
            Debug.WriteLine("Disable");
            DiskManagementService.SetIsOffline(disk.Number, true);
        });

        return item;
    }

    private void RebuildMenu(DisksState state)
    {
        _menu = new ContextMenuStrip();

        foreach (var disk in state.Disks)
        {
            _menu.Items.Add(GetDiskMenuItem(disk));
        }

        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add("Refresh", null, OnExit);

        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add("Exit", null, OnExit);

        _icon.ContextMenuStrip = _menu;
    }
}
