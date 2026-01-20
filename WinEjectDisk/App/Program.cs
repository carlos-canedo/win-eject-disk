namespace WinEjectDisk.App;

static class Program
{
    // FIXME: refactor constants
    static string LOG_FILE_NAME = "log.txt";
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        try
        {
            // FIXME: refactor this logs
            File.Delete(LOG_FILE_NAME);
            File.AppendAllText(LOG_FILE_NAME, "started\n");

            using var tray = new TrayHost();
            Application.Run(new ApplicationContext());

            File.AppendAllText(LOG_FILE_NAME, "finished\n");
        }
        catch (Exception exception)
        {
            File.AppendAllText(LOG_FILE_NAME, exception.ToString());
        }
    }
}
