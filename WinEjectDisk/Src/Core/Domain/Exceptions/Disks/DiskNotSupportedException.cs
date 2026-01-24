namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskNotSupportedException : DiskException
{
    private const string DefaultMessage = "The selected disk is not supported by this operation.";

    public DiskNotSupportedException()
        : base(DefaultMessage) { }

    public DiskNotSupportedException(Exception innerException)
        : base(DefaultMessage, innerException) { }
}
