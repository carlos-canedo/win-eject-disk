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
            Text = "Disable External Disks",
            ContextMenuStrip = _menu,
            Visible = true
        };

        // Open menu on left click
        trayIcon.MouseClick += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo method = typeof(NotifyIcon).GetMethod("ShowContextMenu",
                    BindingFlags.Instance | BindingFlags.NonPublic)!;
                method.Invoke(trayIcon, null);
            }
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
