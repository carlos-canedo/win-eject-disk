using System.Diagnostics;
using WinEjectDisk.Src.Core.Commands.Disk;
using WinEjectDisk.Src.Core.Commands.Disks;
using WinEjectDisk.Src.Core.Constants;
using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Domain.Dtos;
using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.App;

internal sealed class TrayHost : IDisposable
{
    // FIXME: refactor constants
    private readonly NotifyIcon _icon;
    private ContextMenuStrip? _menu;
    private DiskCommandDispatcher _dispatcher;

    public TrayHost()
    {
        // FIXME: use dependency injection packages are already installed
        var consumers = new IDiskConsumer[]
        {
            new SetOnlineConsumer(),
            new SetOfflineConsumer(),
        };

        _dispatcher = new DiskCommandDispatcher(consumers);

        _icon = CreateTrayIcon();
        BuildMenu();
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

    private ToolStripMenuItem GetDiskMenuItem(DiskDto disk)
    {
        var item = new ToolStripMenuItem(disk.Name);

        Logger.Log(disk.Name);

        foreach (var action in disk.Actions)
        {
            // FIXME: action labels should not come in from core
            item.DropDownItems.Add(action.Label, null, (_, _) =>
            {
                Debug.WriteLine($"{action.Label} started");

                _dispatcher.Dispatch(
                    diskNumber: disk.Number,
                    diskHashCode: disk.GetHashCode(),
                    command: action.Command
                );

                Debug.WriteLine($"{action.Label} finished");

                BuildMenu();
            });
        }

        return item;
    }

    private void BuildMenu()
    {
        var disks = new GetDisksQuery().Execute();
        _menu = new ContextMenuStrip();

        foreach (var disk in disks)
        {
            _menu.Items.Add(GetDiskMenuItem(disk));
        }

        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add("Refresh", null, (_, _) =>
        {
            BuildMenu();
        });

        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add("Exit", null, OnExit);

        _icon.ContextMenuStrip = _menu;
    }
}

// FIXME: clean up the project
// FIXME: add error handling - add success and error notifications - should be interesting to add custom errors
