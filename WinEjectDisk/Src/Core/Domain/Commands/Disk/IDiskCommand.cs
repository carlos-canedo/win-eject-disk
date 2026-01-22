namespace WinEjectDisk.Src.Core.Domain.Commands.Disk;

public interface IDiskCommand
{
    DiskCommand Command { get; }
    void Execute(int diskNumber, int diskHashCode);
}
