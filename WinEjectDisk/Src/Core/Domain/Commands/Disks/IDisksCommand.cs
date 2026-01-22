namespace WinEjectDisk.Src.Core.Domain.Commands.Disks;

public interface IDisksCommand
{
    DisksCommand Command { get; }
    void Execute();
}
