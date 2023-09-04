namespace NKelemen18.Uuid.v7;

public class UuidV7GeneratorOptions
{
    public bool UseSequence { get; init; }
    public short SequenceStartMinValue { get; init; }
    public short SequenceStartMaxValue { get; init; }

    public UuidV7GeneratorOptions(
        bool useSequence = true,
        short sequenceStartMinValue = 0,
        short sequenceStartMaxValue = 2048
    )
    {
        if (v7.UuidV7.TickSequenceMaxValue < sequenceStartMaxValue)
            throw new ArgumentOutOfRangeException(nameof(sequenceStartMaxValue), sequenceStartMaxValue,
                $"Sequence start maximum value must be lower than {v7.UuidV7.TickSequenceMaxValue}");


        if (sequenceStartMinValue < 0 || sequenceStartMaxValue <= sequenceStartMinValue)
            throw new ArgumentOutOfRangeException(nameof(sequenceStartMinValue), sequenceStartMinValue,
                $"Sequence start minimum value must be between 0 and {sequenceStartMaxValue}");

        UseSequence = useSequence;
        SequenceStartMinValue = sequenceStartMinValue;
        SequenceStartMaxValue = sequenceStartMaxValue;
    }
}