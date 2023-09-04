using FluentAssertions;
using NKelemen18.Uuid.v7;

namespace NKelemen18.UuidTest.v7;

public class UuidV7GeneratorOptionsTests
{
    [Fact]
    public void DefaultConstructor_ShouldSetPropertiesToDefaultValues()
    {
        // Arrange & Act
        var options = new UuidV7GeneratorOptions();

        // Assert
        options.UseSequence.Should().BeTrue();
        options.SequenceStartMinValue.Should().Be(0);
        options.SequenceStartMaxValue.Should().Be(2048);
    }

    [Theory]
    [InlineData(true, 0, 2048)]
    [InlineData(false, 100, 200)]
    public void ParameterizedConstructor_ShouldSetPropertiesCorrectly(bool useSequence, short minValue, short maxValue)
    {
        // Arrange & Act
        var options = new UuidV7GeneratorOptions(useSequence, minValue, maxValue);

        // Assert
        options.UseSequence.Should().Be(useSequence);
        options.SequenceStartMinValue.Should().Be(minValue);
        options.SequenceStartMaxValue.Should().Be(maxValue);
    }

    [Fact]
    public void Constructor_WithInvalidSequenceStartMaxValue_ShouldThrowException()
    {
        // Arrange & Act & Assert
        FluentActions.Invoking(() => new UuidV7GeneratorOptions(true, 0, Uuid.v7.UuidV7.TickSequenceMaxValue + 1))
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"Sequence start maximum value must be lower than {Uuid.v7.UuidV7.TickSequenceMaxValue}*");
    }

    [Theory]
    [InlineData(-1, 2048)]
    [InlineData(100, 99)]
    public void Constructor_WithInvalidSequenceStartValues_ShouldThrowException(short minValue, short maxValue)
    {
        // Arrange & Act & Assert
        FluentActions.Invoking(() => new UuidV7GeneratorOptions(true, minValue, maxValue))
            .Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage($"Sequence start minimum value must be between 0 and {maxValue}*");
    }
}