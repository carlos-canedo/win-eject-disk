using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Domain.Exceptions;
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
            throw new DiskMismatchException();
        }

        DiskManagementService.SetIsOffline(diskNumber, isOffline: false);
        Logger.Log("Finished set isOffline to false");
        DiskManagementService.SetIsReadOnly(diskNumber, isReadOnly: false);
        Logger.Log("Finished set isReadOnly to false");
    }
}
