using System.Diagnostics;
using System.Management;
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
                    Debug.WriteLine("Enable");
                });
            }
            else
            {
                item.DropDownItems.Add("Disable", null, (_, _) =>
                {
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
            Icon = new Icon(iconPath),
            Text = "UsbDiskToggle",
            ContextMenuStrip = _menu,
            Visible = true
        };

        return trayIcon;
    }

    private List<DiskPs> GetPsDisks()
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

        using var p = Process.Start(psi);
        var json = p.StandardOutput.ReadToEnd();
        var disksPs = JsonSerializer.Deserialize<List<DiskPs>>(json);

        p.WaitForExit();

        return disksPs!;
    }

    private List<DiskMetadata> GetDisks()
    {
        var drives = DriveInfo.GetDrives();
        var removable = DriveInfo.GetDrives()
            .Where(d => d.IsReady && d.DriveType == DriveType.Removable)
            .ToList();

        var disks = new ManagementObjectSearcher(
            "SELECT * FROM Win32_DiskDrive"
        )
            .Get()
            .Cast<ManagementObject>()
            .ToList()
            .Where((disk) =>
            {
                var mediaType = disk["MediaType"]?.ToString();

                bool isExternal =
                    mediaType != null &&
                    mediaType.Contains("External", StringComparison.OrdinalIgnoreCase);

                foreach (PropertyData prop in disk.Properties)
                {
                    Debug.WriteLine($"{prop.Name} = {prop.Value}");
                }

                return isExternal;
            })
            .Select((disk) =>
            {
                var deviceId = disk["DeviceId"].ToString()!;
                var partitions = GetDiskPartitions(deviceId);

                return new DiskMetadata()
                {
                    DeviceId = deviceId,
                    PNPDeviceID = disk["PNPDeviceID"].ToString()!,
                    Index = disk["Index"].ToString()!,
                    Model = disk["Model"].ToString()!,
                    DiskPartitions = partitions,
                };
            })
            .ToList();


        return disks;
    }

    private List<DiskPartition> GetDiskPartitions(string deviceId)
    {
        var partitions = new ManagementObjectSearcher(
            $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceId}'}} " +
            "WHERE AssocClass=Win32_DiskDriveToDiskPartition"
        ).Get();

        var list = new List<DiskPartition>();

        foreach (ManagementObject partition in partitions)
        {
            var logicals = new ManagementObjectSearcher(
                $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} " +
                "WHERE AssocClass=Win32_LogicalDiskToPartition"
            ).Get();

            foreach (ManagementObject logical in logicals)
            {
                string letter = logical["Name"].ToString()!;
                string label = logical["VolumeName"].ToString()!;

                list.Add(new() { ID = "", Label = label, Letter = letter });
            }
        }

        return list;
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
