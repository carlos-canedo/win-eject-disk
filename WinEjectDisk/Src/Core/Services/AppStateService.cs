using WinEjectDisk.Src.Core.Domain.Entities;
using WinEjectDisk.Src.Core.Extensions;

namespace WinEjectDisk.Src.Core.Services;

public sealed class TrayState
{
    public IReadOnlyList<Disk> Disks { get; init; } = [];
    public DateTime LastUpdated { get; init; }
}

public sealed class AppStateService
{
    public TrayState Current { get; private set; } = new();
    public event EventHandler<TrayState>? StateChanged;

    public async Task RefreshAsync()
    {
        var disks = DiskManagementService.GetDisks()
            // .Where((disk) => disk.IsExternal())
            .ToList();

        Current = new TrayState
        {
            Disks = disks,
            LastUpdated = DateTime.UtcNow
        };

        StateChanged?.Invoke(this, Current);
    }
}
