using FluentAssertions;
using NKelemen18.Uuid;
using NKelemen18.Uuid.v7;

namespace NKelemen18.UuidTest.v7;

public class UuidV7DecoderTest
{
    [Fact]
    public void TestParseV4GuidException()
    {
        // Arrange
        var v4Guid = new Guid("930ad408-4b45-11ee-b544-325096b39f47");

        // Act & Assert
        FluentActions.Invoking(() => UuidV7Decoder.Decode(v4Guid))
            .Should()
            .Throw<UuidDecoderException>()
            .WithMessage("Version is incorrect");
    }

    [Fact]
    public void TestParseExampleV7Guid()
    {
        // Arrange
        // https://www.ietf.org/archive/id/draft-ietf-uuidrev-rfc4122bis-10.html#name-example-of-a-uuidv7-value
        // TimeStamp: Tuesday, February 22, 2022 2:22:22.00 PM GMT-05:00, Sequence: 3267
        var exampleV7Guid = new Guid("017F22E2-79B0-7CC3-98C4-DC0C0C07398F");
        // February 22, 2022, 2:22:22.00 PM GMT-05:00" is equivalent to February 22, 2022, 7:22:22.00 PM UTC
        var expectedDateTime = new DateTime(2022, 2, 22, 19, 22, 22, 0, DateTimeKind.Utc);

        // Act
        var (actualDateTime, actualSequence) = UuidV7Decoder.Decode(exampleV7Guid);

        // Assert
        actualDateTime.Should().Be(expectedDateTime);
        actualSequence.Should().Be(3267);
    }

    [Fact]
    public void TestParseV7GuidWithWrongVariantException()
    {
        // Arrange
        var invalidVariantGuid = new Guid("017F22E2-79B0-7CC3-C8C4-DC0C0C07398F");

        // Act & Assert
        FluentActions.Invoking(() => UuidV7Decoder.Decode(invalidVariantGuid))
            .Should()
            .Throw<UuidDecoderException>()
            .WithMessage("Variant is incorrect");
    }

}