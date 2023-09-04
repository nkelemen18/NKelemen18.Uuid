using FluentAssertions;
using NKelemen18.Uuid;
using NKelemen18.Uuid.v7;

namespace NKelemen18.UuidTest.v7;

public class UuidV7GeneratorTest
{
    private readonly DateTime _testTs1 = new(2023, 12, 31, 12, 30, 30, 0, DateTimeKind.Utc);
    private readonly DateTime _testTs2 = new(2023, 12, 31, 12, 30, 30, 1, DateTimeKind.Utc);


    [Fact]
    public void TestCanGenerateGuid()
    {
        // Assert
        var generator = new UuidV7Generator();

        // Act
        var uuid = generator.NewUuidV7();

        // Assert
        uuid.Should().NotBeEmpty();
        uuid.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void TestCanGenerateGuidFromTimestamp()
    {
        // Assert
        var generator = new UuidV7Generator();

        // Act
        var uuid = generator.NewUuidV7(_testTs1);

        // Assert
        uuid.Should().NotBeEmpty();
        uuid.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void TestIsOrdered()
    {
        // Assert
        var generator = new UuidV7Generator();
        var guidList = new List<Guid>(100);

        // Act
        for (var i = 0; i < 100; i++)
        {
            guidList.Add(generator.NewUuidV7());
        }

        // Arrange
        guidList.Select(g => g.ToString()).Should().BeInAscendingOrder();
    }

    [Fact]
    public void TestIsOrderedWithinSameTimestamp()
    {
        // Assert
        var generator = new UuidV7Generator();
        var guidList = new List<Guid>(100);

        // Act
        for (var i = 0; i < 100; i++)
        {
            guidList.Add(generator.NewUuidV7(_testTs1));
        }

        // Arrange
        guidList.Select(g => g.ToString()).Should().BeInAscendingOrder();
    }

    [Fact]
    public void TestIsOrderedDifferentTimestamp()
    {
        // Assert
        var generator = new UuidV7Generator();
        var guidList = new List<Guid>(100);

        // Act
        for (var i = 0; i < 100; i++)
        {
            var ts = new DateTime(2023, 12, 31, 12, 0, 0, i, DateTimeKind.Utc);
            guidList.Add(generator.NewUuidV7(ts));
        }

        // Arrange
        guidList.Select(g => g.ToString()).Should().BeInAscendingOrder();
    }

    [Fact]
    public void TestExceptionOnClockBackTick()
    {
        // Assert
        var generator = new UuidV7Generator();

        // Act && Assert
        FluentActions.Invoking(() => generator.NewUuidV7(_testTs2))
            .Should()
            .NotThrow();

        FluentActions.Invoking(() => generator.NewUuidV7(_testTs1))
            .Should()
            .Throw<UuidGeneratorException>()
            .WithMessage("Clock moved backward");

        // Test that generator is usable after error
        FluentActions.Invoking(() => generator.NewUuidV7(_testTs2))
            .Should()
            .NotThrow();
    }

    [Fact]
    public void TestNoSequenceIsOrderedDifferentTimestamp()
    {
        // Assert
        var options = new UuidV7GeneratorOptions(useSequence: false);
        var generator = new UuidV7Generator(options);
        var guidList = new List<Guid>(100);

        // Act
        for (var i = 0; i < 100; i++)
        {
            var ts = new DateTime(2023, 12, 31, 12, 0, 0, i, DateTimeKind.Utc);
            guidList.Add(generator.NewUuidV7(ts));
        }

        // Arrange
        guidList.Select(g => g.ToString()).Should().BeInAscendingOrder();
    }

    [Fact]
    public void TestSequenceOverflowWhenUseFixTimestamp()
    {
        // Assert
        var options = new UuidV7GeneratorOptions(
            sequenceStartMinValue: UuidV7.TickSequenceMaxValue - 5,
            sequenceStartMaxValue: UuidV7.TickSequenceMaxValue
        );
        var generator = new UuidV7Generator(options);

        // Act && Arrange
        FluentActions.Invoking(() =>
            {
                for (var i = 0; i < 10; i++)
                    generator.NewUuidV7(_testTs2);
            })
            .Should()
            .Throw<UuidGeneratorException>()
            .WithMessage("Tick sequence overflow");
    }

    [Fact]
    public void TestNoSequenceOverflowWhenUseSystemTime()
    {
        // Assert
        var options = new UuidV7GeneratorOptions(
            sequenceStartMinValue: UuidV7.TickSequenceMaxValue - 5,
            sequenceStartMaxValue: UuidV7.TickSequenceMaxValue
        );
        var generator = new UuidV7Generator(options);

        // Act && Arrange
        FluentActions.Invoking(() =>
            {
                for (var i = 0; i < 100; i++)
                    generator.NewUuidV7();
            })
            .Should()
            .NotThrow();
    }
}