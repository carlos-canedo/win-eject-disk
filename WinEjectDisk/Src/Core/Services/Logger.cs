using WinEjectDisk.Src.Core.Constants;

namespace WinEjectDisk.Src.Core.Services;

// FIXME: use microsoft.logging instead
public static class Logger
{
    private static readonly object _lock = new();

    private static readonly string _logPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Config.AppName,
            Config.LogFilePath
        );

    public static void Log(string message)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath)!);

            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
            var something = _logPath;

            lock (_lock)
            {
                File.AppendAllText(_logPath, line + Environment.NewLine);
            }
        }
        catch
        {
            // never crash because of logging
        }
    }
}
