namespace WinEjectDisk.Src.Core.Extensions;

public static class StringExtensions
{
    public static bool IsInList(this string str, IEnumerable<string> list)
    {
        if (string.IsNullOrEmpty(str)) return false;

        return list.Any(keyword =>
            str.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}
