namespace WinEjectDisk.Tests.Src.Domain;

public class DiskTests
{
  [Fact]
  public void Test1()
  {
    var mock = new Mock<IDisposable>();
    mock.Object.Should().NotBeNull();
  }
}
