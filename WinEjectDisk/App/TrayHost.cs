internal sealed class TrayHost : IDisposable
{
    private readonly string TRAY_ICON_PATH = "TrayIcon.ico";
    private readonly NotifyIcon _tray;
    private readonly ContextMenuStrip _menu;

    public TrayHost()
    {
        _menu = new ContextMenuStrip();
        _menu.Items.Add("Exit", null, OnExit);

        string path = Path.Combine(
            AppContext.BaseDirectory,
            TRAY_ICON_PATH
        );

        string iconPath = Path.Combine(AppContext.BaseDirectory, TRAY_ICON_PATH);

        _tray = new NotifyIcon
        {
            // Icon = SystemIcons.Application,
            Icon = new Icon(iconPath),
            Text = "UsbDiskToggle",
            ContextMenuStrip = _menu,
            Visible = true
        };
    }

    private void OnExit(object? sender, EventArgs e)
    {
        Dispose();
        Application.Exit();
    }

    public void Dispose()
    {
        _tray.Visible = false;
        _tray.Dispose();
        _menu.Dispose();
    }
}
