using System.Reflection.Emit;
using WinEjectDisk.Src.Core.Domain.Dtos;
using WinEjectDisk.Src.Core.Domain.Entities;
using WinEjectDisk.Src.Core.Extensions;

namespace WinEjectDisk.Src.Core.Services;

// FIXME: move this
public sealed class DisksState
{
    public IReadOnlyList<DiskDto> Disks { get; init; } = [];
    public DateTime LastUpdated { get; init; }
}

public sealed class DisksStateService
{
    public DisksState Current { get; private set; } = new();
    public event EventHandler<DisksState>? StateChanged;

    public void RefreshDisks()
    {
        var disks = GetDisks();

        Current = new DisksState
        {
            Disks = disks,
            LastUpdated = DateTime.UtcNow
        };

        StateChanged?.Invoke(this, Current);
    }

    private List<DiskDto> GetDisks()
    {
        var disks = DiskManagementService.GetDisks()
            .Where((disk) => disk.IsExternal())
            .Select((disk) =>
            {
                string label = $"{disk.FriendlyName} ({disk.FriendlySize})";


                return new DiskDto()
                {
                    Number = disk.Number,
                    HashCode = disk.GetHashCode(),
                    Label = label,
                    Actions = GetListActions(disk),
                };
            })
            .ToList();

        return disks;
    }

    private List<DiskActionDto> GetListActions(Disk disk)
    {
        var actions = new List<DiskActionDto>();

        if (disk.IsOffline)
        {
            actions.Add(new()
            {
                Label = "Enable",
                Action = DiskAction.SetOnline,
            });
        }

        if (!disk.IsOffline)
        {
            actions.Add(new()
            {
                Label = "Disable",
                Action = DiskAction.SetOffline,
            });
        }

        return actions;
    }
}
