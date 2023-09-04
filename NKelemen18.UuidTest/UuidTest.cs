using FluentAssertions;

namespace NKelemen18.UuidTest;

public class UuidTest
{
    [Fact]
    public void TestNilUuid()
    {
        // Assert
        Uuid.Uuid.Nil.Should().Be(Guid.Empty);
        Uuid.Uuid.Nil.Should().Be(new Guid("00000000-0000-0000-0000-000000000000"));
    }

    [Fact]
    public void TestMaxUuid()
    {
        // Assert
        Uuid.Uuid.Max.Should().Be(new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"));
    }
}