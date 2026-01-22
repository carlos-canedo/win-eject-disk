using WinEjectDisk.Src.Core.Domain.Commands.Disk;

namespace WinEjectDisk.Src.Core.Commands.Disk;

public sealed class SetOfflineConsumer : IDiskCommand
{
    public DiskCommand Command => DiskCommand.SetOffline;

    public void Execute(int diskNumber, int diskHashCode)
    {
        throw new NotImplementedException();
    }
}
