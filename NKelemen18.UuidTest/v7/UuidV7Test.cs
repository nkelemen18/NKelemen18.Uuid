using FluentAssertions;
using NKelemen18.Uuid.v7;

namespace NKelemen18.UuidTest.v7;

public class UuidV7Test
{
    [Fact]
    public void TestCanGenerateGuid()
    {
        // Act
        var uuid = UuidV7.NewUuidV7();

        // Assert
        uuid.Should().NotBeEmpty();
        uuid.Should().NotBe(Guid.Empty);
    }
}