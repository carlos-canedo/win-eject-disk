using System.Diagnostics;
using System.Text.Json;
using WinEjectDisk.Src.Core.Domain.Entities;
using WinEjectDisk.Src.Core.Factories;

namespace WinEjectDisk.Src.Core.Services;

public static class DiskManagementService
{
    private const string _getDisksCommand = "-Command \"ConvertTo-Json -Depth 1 -InputObject @(Get-Disk)\"";
    private const string _getDiskByIdCommand = "-Command \"Get-Disk -Number {0} | ConvertTo-Json\"";
    private const string _setIsOfflineCommand = "-Command \"Set-Disk -Number {0} -IsOffline ${1}\"";
    private const string _setIsReadOnlyCommand = "-Command \"Set-Disk -Number {0} -IsReadOnly ${1}\"";

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
    
    public static void SetIsReadOnly(int diskNumber, bool isReadOnly)
    {
        var command = string.Format(
            _setIsReadOnlyCommand,
            diskNumber,
            isReadOnly.ToString().ToLower()
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
        var command = string.Format(
            _getDiskByIdCommand,
            diskNumber
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

        var json = ExecuteProcessStartInfo(psi);
        var disk = JsonSerializer.Deserialize<Disk>(json);

        return disk!;
    }

    private static string ExecuteProcessStartInfo(ProcessStartInfo psi)
    {
        using var process = Process.Start(psi)!;

        string stdout = process.StandardOutput.ReadToEnd();
        string stderr = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (process.ExitCode != 0 || !string.IsNullOrEmpty(stderr))
        {
            // Disk exceptions handler
            var exception = DiskExceptionFactory.CreateFromPayload(stderr);

            Logger.Log(exception.Message);
            throw exception;
        }

        return stdout;
    }
}
