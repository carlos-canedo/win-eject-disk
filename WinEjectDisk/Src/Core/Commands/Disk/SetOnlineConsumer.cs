using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.Core.Commands.Disk;

public sealed class SetOnlineConsumer : IDiskConsumer
{
    public DiskCommand Command => DiskCommand.SetOnline;

    public void Execute(int diskNumber, int diskHashCode)
    {
        var pendingDisk = DiskManagementService.GetDiskByNumber(diskNumber);

        if (diskHashCode != pendingDisk.GetHashCode())
        {
            // FIXME: update text
            throw new Exception("The disk does not coincide, pls refresh and try again");
        }

        DiskManagementService.SetIsOffline(diskNumber, isOffline: false);
    }
}
