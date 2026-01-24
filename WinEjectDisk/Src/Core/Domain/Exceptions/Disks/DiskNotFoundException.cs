using WinEjectDisk.Src.Core.Extensions;

namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskNotFoundException : DiskException
{
    private static readonly string[] ErrorKeywords = ["Set-Disk : Not Supported"];
    private const string DefaultMessage = "The system could not find the specified disk. It may have been disconnected.";

    public DiskNotFoundException()
        : base(DefaultMessage) { }

    public DiskNotFoundException(Exception innerException)
        : base(DefaultMessage, innerException) { }

    public static DiskNotFoundException? FromPayload(string payload)
    {
        return payload.IsInList(ErrorKeywords)
            ? new DiskNotFoundException(new Exception(payload))
            : null;
    }
}
