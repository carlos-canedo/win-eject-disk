using WinEjectDisk.Src.Core.Domain.Commands.Disk;

namespace WinEjectDisk.Src.Core.Domain.Dtos;

public sealed class DiskActionDto
{
    public string Label { get; init; } = string.Empty;
    public DiskCommand Command { get; init; }
}
