using WinEjectDisk.Src.Core.Domain.Entities;

namespace WinEjectDisk.Src.Core.Domain.Dtos;

public sealed class DiskActionDto
{
    public string Label { get; init; } = string.Empty;
    public DiskAction Action { get; init; }
}
