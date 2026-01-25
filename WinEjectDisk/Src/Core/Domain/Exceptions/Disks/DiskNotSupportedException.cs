using WinEjectDisk.Src.Core.Extensions;

namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskNotSupportedException : DiskException
{
    private const string DefaultMessage = "The selected disk is not supported by this operation.";
    private static readonly string[] ErrorKeywords = ["Set-Disk : Not Supported"];

    public DiskNotSupportedException()
        : base(DefaultMessage) { }

    public DiskNotSupportedException(Exception innerException)
        : base(DefaultMessage, innerException) { }

    public static DiskNotSupportedException? FromPayload(string payload)
    {
        return payload.IsInList(ErrorKeywords)
            ? new DiskNotSupportedException(new Exception(payload))
            : null;
    }
}
