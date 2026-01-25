using WinEjectDisk.Src.Core.Extensions;

namespace WinEjectDisk.Src.Core.Domain.Exceptions;

public class DiskAccessDeniedException : DiskException
{
    private const string DefaultMessage = "Access denied. Please run the application as Administrator to modify disk settings.";
    private static readonly string[] ErrorKeywords = ["Set-Disk : Access Denied"];

    public DiskAccessDeniedException()
        : base(DefaultMessage) { }

    public DiskAccessDeniedException(Exception innerException)
        : base(DefaultMessage, innerException) { }

    public static DiskAccessDeniedException? FromPayload(string payload)
    {
        return payload.IsInList(ErrorKeywords)
            ? new DiskAccessDeniedException(new Exception(payload))
            : null;
    }
}
