using WinEjectDisk.Src.App.Contracts;
using WinEjectDisk.Src.App.Services;
using WinEjectDisk.Src.Core.Commands.Disk;
using WinEjectDisk.Src.Core.Domain.Commands.Disk;
using WinEjectDisk.Src.Core.Domain.Dtos;
using WinEjectDisk.Src.Core.Domain.Exceptions;
using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.App.Controllers;

public sealed class DiskActionController : IDiskActionController
{
    private DiskCommandDispatcher _dispatcher;
    private IDialogService _dialogService;

    public DiskActionController()
    {
        // FIXME: use dependency injection packages are already installed
        var consumers = new IDiskConsumer[]
        {
            new SetOnlineConsumer(),
            new SetOfflineConsumer(),
        };

        _dispatcher = new DiskCommandDispatcher(consumers);
        _dialogService = new DialogService();
    }

    public void ExecuteAction(DiskDto disk, DiskCommand command)
    {
        if (command == DiskCommand.SetOffline || command == DiskCommand.SetOnline)
        {
            ChangeDiskOnlineState(disk, command);
        }
    }

    private void ChangeDiskOnlineState(DiskDto disk, DiskCommand command)
    {
        string action = command == DiskCommand.SetOnline ? "enabled" : "disabled";

        try
        {
            Logger.Log($"{action} started");

            _dispatcher.Dispatch(
                diskNumber: disk.Number,
                diskHashCode: disk.HashCode,
                command: command
            );

            Logger.Log($"{action} finished");

            string title = $"Disk {action} successfully";
            string message = $"Your disk has been {action}";

            _dialogService.ShowInfo(title: title, message: message);

            // FIXME: Need to rebuild the menu but not from here because it will cause a circular dependency
        }
        catch (DiskException exception)
        {
            Logger.Log($"{action} finished with error: ${exception}");

            string title = "We couldn't disable your disk";
            _dialogService.ShowError(title: title, message: exception.Message);
        }
        catch (Exception exception)
        {
            Logger.Log($"{action} finished with error: ${exception}");

            string title = "We couldn't disable your disk";
            string message = $"An unknown error happend while trying to disable your disk. Click refresh and try again, if the error persist please check the logs for more details";

            _dialogService.ShowError(title: title, message: message);
        }
    }
}
