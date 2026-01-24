namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskException : Exception
{
    private const string DefaultMessage = "An unexpected error occurred during a disk operation.";

    public DiskException() : base(DefaultMessage) { }

    public DiskException(Exception innerException) : base(DefaultMessage, innerException) { }

    protected DiskException(string message) : base(message) { }

    protected DiskException(string message, Exception innerException)
        : base(message, innerException) { }

    // FIXME: Check this and move it to an extension? i don't know if this logic is ok here
    // public static Exception Resolve(string payload, string diskId)
    // {
    //     // This acts as a "Router"
    //     if (payload.Contains("AccessDenied")) return new DiskAccessDeniedException();
    //     if (payload.Contains("NotFound"))     return new DiskNotFoundException(diskId);
    //     if (payload.Contains("NotSupported"))  return new DiskNotSupportedException();
        
    //     // If no matches, return the base DiskException
    //     return new DiskException("An unknown disk error occurred.", new Exception(payload));
    // }
}
