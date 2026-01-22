using System.Diagnostics;
using System.Text.Json;
using WinEjectDisk.Src.Core.Domain.Entities;

namespace WinEjectDisk.Src.Core.Services;

public static class DiskManagementService
{
    private const string _getDisksCommand = "-Command \"Get-Disk | ConvertTo-Json -Depth 3\"";
    private const string _setIsOfflineCommand = "-Command \"Set-Disk -Number {0} -IsOffline ${1}\"";
    private const string _notSupportedErrorKeyword = "Set-Disk : Not Supported";

    public static List<Disk> GetDisks()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = _getDisksCommand,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        var json = ExecuteProcessStartInfo(psi);
        var disks = JsonSerializer.Deserialize<List<Disk>>(json);

        return disks!;
    }

    public static void SetIsOffline(int diskNumber, bool isOffline)
    {
        var command = string.Format(
            _setIsOfflineCommand,
            diskNumber,
            isOffline.ToString().ToLower()
        );

        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = command,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        ExecuteProcessStartInfo(psi);
    }

    public static Disk GetDiskByNumber(int diskNumber)
    {
        return GetDisks().First((disk) => disk.Number == diskNumber);
    }

    private static string ExecuteProcessStartInfo(ProcessStartInfo psi)
    {
        using var process = Process.Start(psi)!;

        string stdout = process.StandardOutput.ReadToEnd();
        string stderr = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0 || !string.IsNullOrEmpty(stderr))
        {
            // FIXME: should throw custom exception
            string errorMessage = stderr.Contains(_notSupportedErrorKeyword)
                ? "Disk doesn't support offline"
                : "Disk offline status failed for an unknown issue";

            Logger.Log(errorMessage);
            throw new Exception(errorMessage);
        }

        return stdout;
    }
}
