namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskAccessDeniedException : DiskException
{
    private const string DefaultMessage = "Access denied. Please run the application as Administrator to modify disk settings.";

    public DiskAccessDeniedException()
        : base(DefaultMessage) { }

    public DiskAccessDeniedException(Exception innerException)
        : base(DefaultMessage, innerException) { }
}
