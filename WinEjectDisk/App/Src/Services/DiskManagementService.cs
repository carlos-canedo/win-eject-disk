using System.Diagnostics;
using System.Text.Json;
using WinEjectDisk.App.Domain;

namespace WinEjectDisk.App.Services
{
  public static class DiskManagementService
  {
    private const string _getDisksCommand = "-Command \"Get-Disk | ConvertTo-Json -Depth 3\"";
    private const string _setIsOfflineCommand = "-Command \"Set-Disk -Number {0} -IsOffline ${1}\"";

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

      using var process = Process.Start(psi)!;
      var json = process.StandardOutput.ReadToEnd();
      var disks = JsonSerializer.Deserialize<List<Disk>>(json);

      process.WaitForExit();

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

      using var process = Process.Start(psi)!;
      process.WaitForExit();
    }

    public static Disk GetDiskByNumber(int diskNumber)
    {
      return GetDisks().First((disk) => disk.Number == diskNumber);
    }
  }
}
