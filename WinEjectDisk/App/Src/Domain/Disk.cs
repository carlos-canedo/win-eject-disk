namespace WinEjectDisk.App.Domain
{
  public class Disk
  {
    public int Number { get; set; }
    public long Size { get; set; }
    public string FriendlyName { get; set; } = string.Empty;
    public string BusType { get; set; } = string.Empty;
    public bool IsOffline { get; set; }
    public bool IsSystem { get; set; }
    public bool IsBoot { get; set; }
  }
}
