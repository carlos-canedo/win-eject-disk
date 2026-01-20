namespace WinEjectDisk.App
{

  public class DiskPs
  {
    public int Number { get; set; }
    public long Size { get; set; }
    public string FriendlyName { get; set; }
    public string BusType { get; set; }
    public bool IsOffline { get; set; }
    public bool IsSystem { get; set; }
    public bool IsBoot { get; set; }
  }

  public class DiskMetadata
  {
    public required string DeviceId;
    public required string PNPDeviceID;
    public required string Index;
    public required string Model;
    public List<DiskPartition> DiskPartitions = new();
  }

  public class DiskPartition
  {
    public required string ID;
    public required string Label;
    public required string Letter;
  }
}