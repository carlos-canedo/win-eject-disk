using System.Diagnostics;
using System.Text.Json;
using WinEjectDisk.App;

internal sealed class TrayHost : IDisposable
{
    // FIXME: refactor constants
    private readonly string TRAY_ICON_PATH = "TrayIcon.ico";
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
        var disks = GetPsDisks();

        var validBusTypes = new[] { "USB", "SD" };
        var externalDisks = disks.Where(d =>
            !d.IsBoot &&
            !d.IsSystem &&
            validBusTypes.Contains(d.BusType)
        );

        foreach (var disk in externalDisks)
        {
            var item = new ToolStripMenuItem(disk.FriendlyName);
            if (disk.IsOffline)
            {
                item.DropDownItems.Add("Enable", null, (_, _) =>
                {
                    SetIsOffline(disk.Number, false);
                    Debug.WriteLine("Enable");
                });
            }
            else
            {
                item.DropDownItems.Add("Disable", null, (_, _) =>
                {
                    SetIsOffline(disk.Number, true);
                    Debug.WriteLine("Disable");
                });
            }

            menu.Items.Add(item);
        }
    }

    private NotifyIcon CreateTrayIcon()
    {
        string iconPath = Path.Combine(AppContext.BaseDirectory, TRAY_ICON_PATH);

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

    private void SetIsOffline(int diskNumber, bool isOffline)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-Command \"Set-Disk -Number {diskNumber} -IsOffline ${isOffline.ToString().ToLower()}\"",
            Verb = "runas", //FIXME: admin rights
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        using var process = Process.Start(psi)!;
        process.WaitForExit();
    }

    private List<PsDisk> GetPsDisks()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = "-Command \"Get-Disk | ConvertTo-Json -Depth 3\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        using var process = Process.Start(psi)!;
        var json = process.StandardOutput.ReadToEnd();
        var disksPs = JsonSerializer.Deserialize<List<PsDisk>>(json);

        process.WaitForExit();

        return disksPs!;
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
