using WinEjectDisk.Src.Core.Domain.Entities;

namespace WinEjectDisk.Tests.Core.Domain.Entities;

public class DiskTests
{
    [Fact]
    public void Equals_WhenRightIsNull_ReturnsFalse()
    {
        var left = new Disk { Number = 1 };

        left.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenNumbersDiffer_ReturnsFalse()
    {
        var left = new Disk { Number = 1 };
        var right = new Disk { Number = 2 };

        left.Equals(right).Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenBothHaveUniqueId_AndTheyMatch_ReturnsTrue()
    {
        var left = new Disk
        {
            Number = 1,
            UniqueId = "ABC",
            Size = 100
        };

        var right = new Disk
        {
            Number = 1,
            UniqueId = "ABC",
            Size = 200 // size should not matter here
        };

        left.Equals(right).Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenBothHaveUniqueId_ButTheyDiffer_ReturnsFalse()
    {
        var left = new Disk { Number = 1, UniqueId = "ABC" };
        var right = new Disk { Number = 1, UniqueId = "DEF" };

        left.Equals(right).Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenUniqueIdMissing_UsesSizeComparison()
    {
        var left = new Disk
        {
            Number = 1,
            UniqueId = string.Empty,
            Size = 500
        };

        var right = new Disk
        {
            Number = 1,
            UniqueId = string.Empty,
            Size = 500
        };

        left.Equals(right).Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenUniqueIdMissing_AndSizeDiffers_ReturnsFalse()
    {
        var left = new Disk { Number = 1, Size = 500 };
        var right = new Disk { Number = 1, Size = 1000 };

        left.Equals(right).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WhenUniqueIdPresent_UsesUniqueId()
    {
        var disk = new Disk
        {
            Number = 1,
            UniqueId = "ABC",
            Size = 100
        };

        disk.GetHashCode().Should().Be("ABC".GetHashCode());
    }

    [Fact]
    public void GetHashCode_WhenUniqueIdMissing_UsesSize()
    {
        var disk = new Disk
        {
            Number = 1,
            UniqueId = string.Empty,
            Size = 100
        };

        disk.GetHashCode().Should().Be("100".GetHashCode());
    }
}
