namespace WinEjectDisk.Src.Core.Domain.Dtos;

public sealed class DiskDto
{
    public int Number { get; init; }
    public int HashCode { get; init; }
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<DiskActionDto> Actions { get; init; } = [];
}
