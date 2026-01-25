namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskException : Exception
{
    private const string DefaultMessage = "An unexpected error occurred during a disk operation.";

    public DiskException() : base(DefaultMessage) { }

    public DiskException(Exception innerException) : base(DefaultMessage, innerException) { }

    protected DiskException(string message) : base(message) { }

    protected DiskException(string message, Exception innerException)
        : base(message, innerException) { }
}
