using WinEjectDisk.Src.Core.Extensions;

namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskNotFoundException : DiskException
{
    private const string DefaultMessage = "The system could not find the specified disk. It may have been disconnected.";
    private static readonly string[] ErrorKeywords = [
        "Get-Disk : No MSFT_Disk objects found",
        "Set-Disk : The requested object could not be found"
    ];

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
