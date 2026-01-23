namespace WinEjectDisk.Src.App.Contracts;

public interface IMenuController
{
    event EventHandler<ContextMenuStrip>? OnRefresh;
    event EventHandler? OnExit;
    ContextMenuStrip BuildMenu();
}
