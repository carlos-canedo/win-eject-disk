using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Domain.Exceptions;
using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.Core.Commands.Disk;

public sealed class SetOfflineConsumer : IDiskConsumer
{
    public DiskCommand Command => DiskCommand.SetOffline;

    public void Execute(int diskNumber, int diskHashCode)
    {
        var pendingDisk = DiskManagementService.GetDiskByNumber(diskNumber);

        if (diskHashCode != pendingDisk.GetHashCode())
        {
            throw new DiskMismatchException();
        }

        DiskManagementService.SetIsOffline(diskNumber, isOffline: true);
        Logger.Log("Finished set isOffline to true");
    }
}
