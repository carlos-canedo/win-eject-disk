using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Domain.Dtos;

namespace WinEjectDisk.Src.App.Contracts;

public interface IDiskActionController
{
    void ExecuteAction(DiskDto disk, DiskCommand command);
}
