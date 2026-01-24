namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskMismatchException : DiskException
{
    private const string DefaultMessage = "Operation aborted: The target disk does not match the disk selected for update.";

    public DiskMismatchException()
        : base(DefaultMessage) { }

    public DiskMismatchException(Exception innerException)
        : base(DefaultMessage, innerException) { }
}

