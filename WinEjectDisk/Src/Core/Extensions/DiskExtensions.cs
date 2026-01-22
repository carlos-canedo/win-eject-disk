using WinEjectDisk.Src.Core.Constants;
using WinEjectDisk.Src.Core.Domain.Entities;

namespace WinEjectDisk.Src.Core.Extensions;

public static class DiskExtensions
{
    public static bool IsExternal(this Disk disk)
    {
        return !disk.IsBoot &&
          !disk.IsSystem &&
          Config.ExternalBusTypes.Contains(disk.BusType);
    }
}
