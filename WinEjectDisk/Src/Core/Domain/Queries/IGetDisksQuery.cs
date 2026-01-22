using WinEjectDisk.Src.Core.Domain.Dtos;

namespace WinEjectDisk.Src.Core.Domain.Queries;

public interface IGetDisksQuery
{
    IReadOnlyList<DiskDto> Execute();
}
