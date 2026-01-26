using WinEjectDisk.Src.Core.Services;

namespace WinEjectDisk.Src.App;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Logger.Log("started");

        try
        {
            ApplicationConfiguration.Initialize();

            using var tray = new TrayController().BuildTray();
            Application.Run(new ApplicationContext());
        }
        catch (Exception exception)
        {
            Logger.Log(exception.ToString());
        }

        Logger.Log("finished");
    }
}

// FIXME: should be good to move the whole WinEjectDisk as root of this repo, although idk if that's ok. We need to reduce the amount of root folders like dist/install, etc.
