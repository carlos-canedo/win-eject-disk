using System.Reflection;
using WinEjectDisk.Src.App.Contracts;
using WinEjectDisk.Src.App.Controllers;

namespace WinEjectDisk.Src.App;

internal sealed class TrayController : ITrayController, IDisposable
{
    private IMenuController _menuController;
    private NotifyIcon? _icon;
    private ContextMenuStrip? _menu;

    public TrayController()
    {
        _menuController = new MenuController();
    }

    public NotifyIcon BuildTray()
    {
        _icon = CreateTrayIcon();

        // Setup menu controller events
        _menuController.OnRefresh += OnRefresh;
        _menuController.OnExit += OnExit;

        // Build and attach menu
        _menu = _menuController.BuildMenu();
        _icon.ContextMenuStrip = _menu;

        return _icon;
    }

    private NotifyIcon CreateTrayIcon()
    {
        // FIXME: move this to a config file in the app module
        string resourcePath = "WinEjectDisk.Src.App.Assets.TrayIcon.ico";
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? iconStream = assembly.GetManifestResourceStream(resourcePath);

        if (iconStream == null)
        {
            throw new Exception($"Could not find resource at {resourcePath}");
        }

        var trayIcon = new NotifyIcon
        {
            // FIXME: Refactor constants
            Icon = new Icon(iconStream),
            Text = "Eject External Disks",
            ContextMenuStrip = _menu,
            Visible = true
        };

        return trayIcon;
    }

    private void OnRefresh(object? sender, ContextMenuStrip menu)
    {
        _menu = menu;
        if (_icon != null)
        {
            _icon.ContextMenuStrip = menu;
        }
    }

    private void OnExit(object? sender, EventArgs eventArgs)
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_icon != null)
        {
            _icon.Visible = false;
            _icon.Dispose();
        }

        _menu?.Dispose();
        Application.Exit();
    }
}

// FIXME: 
// FIXME: clean up the project
// FIXME: add error handling - add success and error notifications - should be interesting to add custom errors
