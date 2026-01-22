using WinEjectDisk.Src.Core.Domain.Commands.Disk;

namespace WinEjectDisk.Src.Core.Commands.Disk;

public sealed class DiskCommandDispatcher
{
    private readonly Dictionary<DiskCommand, IDiskCommand> _consumers;

    public DiskCommandDispatcher(IEnumerable<IDiskCommand> commands)
    {
        _consumers = commands.ToDictionary(c => c.Command);
    }

    public void Dispatch(int diskNumber, int diskHashCode, DiskCommand command)
    {
        if (_consumers.TryGetValue(command, out var consumer))
        {
            consumer.Execute(diskNumber: diskNumber, diskHashCode: diskHashCode);
        }
    }
}
