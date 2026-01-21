namespace WinEjectDisk.Src.Core.Domain.Entities;

public class Disk : IEquatable<Disk>
{
    public int Number { get; set; }
    public string UniqueId { get; set; } = string.Empty;
    public long Size { get; set; }
    public string FriendlyName { get; set; } = string.Empty;
    public string BusType { get; set; } = string.Empty;
    public bool IsOffline { get; set; }
    public bool IsSystem { get; set; }
    public bool IsBoot { get; set; }

    // Computed props
    public string FriendlySize
    {
        get => $"{Size / (1024d * 1024 * 1024):0.##} GB";
    }

    // Equatable methods
    public bool Equals(Disk? right)
    {
        if (right is null || Number != right.Number)
            return false;

        if (!string.IsNullOrEmpty(UniqueId) && !string.IsNullOrEmpty(right.UniqueId))
        {
            return UniqueId == right.UniqueId;
        }

        return Size == right.Size;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Disk);
    }

    public override int GetHashCode()
    {
        var key = !string.IsNullOrEmpty(UniqueId)
            ? UniqueId
            : Size.ToString();

        return key.GetHashCode();
    }
}
