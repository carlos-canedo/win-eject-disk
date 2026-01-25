using WinEjectDisk.Src.Core.Domain.Exceptions;

namespace WinEjectDisk.Src.Core.Factories;

public static class DiskExceptionFactory
{
    public static DiskException CreateFromPayload(string payload)
    {
        return (DiskException?)DiskNotFoundException.FromPayload(payload)
            ?? (DiskException?)DiskNotSupportedException.FromPayload(payload)
            ?? (DiskException?)DiskAccessDeniedException.FromPayload(payload)
            ?? new DiskException();
    }
}
