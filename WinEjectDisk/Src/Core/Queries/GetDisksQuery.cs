using DomainDisk = WinEjectDisk.Src.Core.Domain.Entities.Disk;
using WinEjectDisk.Src.Core.Domain.Dtos;
using WinEjectDisk.Src.Core.Services;
using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Extensions;
using WinEjectDisk.Src.Core.Domain.Queries;

namespace WinEjectDisk.Src.Core.Commands.Disks;

public sealed class GetDisksQuery : IGetDisksQuery
{
    public IReadOnlyList<DiskDto> Execute()
    {
        return GetDisks();
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
                    Name = label,
                    Actions = GetListActions(disk),
                };
            })
            .ToList();

        return disks;
    }

    private List<DiskActionDto> GetListActions(DomainDisk disk)
    {
        var actions = new List<DiskActionDto>();

        if (disk.IsOffline)
        {
            // FIXME: refactor constants and every palce with quotes ""
            actions.Add(new()
            {
                Label = "Enable",
                Command = DiskCommand.SetOnline,
            });
        }

        if (!disk.IsOffline)
        {
            actions.Add(new()
            {
                Label = "Disable",
                Command = DiskCommand.SetOffline,
            });
        }

        return actions;
    }
}
