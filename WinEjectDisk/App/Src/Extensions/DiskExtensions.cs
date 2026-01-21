using WinEjectDisk.App.Constants;
using WinEjectDisk.App.Domain;

namespace WinEjectDisk.App.Extensions
{
  public static class DiskExtensions
  {
    public static bool IsExternal(this Disk disk)
    {
      return !disk.IsBoot &&
        !disk.IsSystem &&
        Config.ExternalBusTypes.Contains(disk.BusType);
    }

    // FIXME: Add extension for disks to check if the disk number is still the same or maybe with the equal part in the Disk class it already works
  }
}
