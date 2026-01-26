using WinEjectDisk.Src.App.Contracts;
using WinEjectDisk.Src.Core.Commands.Disks;
using WinEjectDisk.Src.Core.Domain.Dtos;
using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.App.Controllers;

public sealed class MenuController : IMenuController
{
    DiskActionController _diskActionController;
    public event EventHandler<ContextMenuStrip>? OnRefresh;
    public event EventHandler? OnExit;

    public MenuController()
    {
        _diskActionController = new DiskActionController();
    }

    public ContextMenuStrip BuildMenu()
    {
        var disks = new GetDisksQuery().Execute();
        var menu = new ContextMenuStrip();

        // Disk actions
        foreach (var disk in disks)
        {
            menu.Items.Add(GetDiskMenuItem(disk));
        }

        // Global actions
        if (disks.Count() > 0)
        {
            menu.Items.Add(new ToolStripSeparator());
        }

        menu.Items.Add(GetRefreshMenuItem());
        menu.Items.Add(GetExitMenuItem());

        return menu;
    }

    private ToolStripMenuItem GetRefreshMenuItem()
    {
        var item = new ToolStripMenuItem("Refresh", null, (_, _) =>
        {
            Logger.Log("Started refresh");

            var menu = BuildMenu();
            OnRefresh?.Invoke(this, menu);

            Logger.Log("Finished refresh");
        });

        return item;
    }

    private ToolStripMenuItem GetExitMenuItem()
    {
        var item = new ToolStripMenuItem("Exit", null, (_, _) =>
        {
            Logger.Log("Started exit");

            OnExit?.Invoke(this, EventArgs.Empty);

            Logger.Log("Finished exit");
        });

        return item;
    }

    private ToolStripMenuItem GetDiskMenuItem(DiskDto disk)
    {
        var item = new ToolStripMenuItem(disk.Name);

        Logger.Log(disk.Name);

        foreach (var action in disk.Actions)
        {
            // FIXME: action labels should not come from core
            item.DropDownItems.Add(action.Label, null, (_, _) =>
            {
                _diskActionController.ExecuteAction(disk, action.Command);
            });
        }

        return item;
    }
}
