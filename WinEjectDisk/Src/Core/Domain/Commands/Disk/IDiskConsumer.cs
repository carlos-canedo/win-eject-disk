namespace WinEjectDisk.Src.Core.Domain.Commands.Disk;

public interface IDiskConsumer
{
    DiskCommand Command { get; }
    void Execute(int diskNumber, int diskHashCode);
}
