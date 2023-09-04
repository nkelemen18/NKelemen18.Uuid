using System.Buffers.Binary;
using System.Security.Cryptography;

namespace NKelemen18.Uuid.v7;

public class UuidV7Generator
{
    private readonly UuidV7GeneratorOptions _opts;
    private readonly object _lockObject = new();
    private short _tickSequence;
    private long _lastTickMs;

    public UuidV7Generator()
    {
        _opts = new UuidV7GeneratorOptions();
    }

    public UuidV7Generator(UuidV7GeneratorOptions options)
    {
        _opts = options;
    }

    public Guid NewUuidV7()
    {
        return NewUuidV7(DateTime.UtcNow, false);
    }

    public Guid NewUuidV7(DateTime utcTimestamp)
    {
        return NewUuidV7(utcTimestamp, true);
    }

    private Guid NewUuidV7(DateTime utcTimestamp, bool fixedTimestamp)
    {
        // Allocate 16 byte for GUID bytes
        Span<byte> bytes = stackalloc byte[16];

        var unixTimestampInMs = GetUnixTimestampInMs(utcTimestamp);


        if (_opts.UseSequence)
        {

            // Fill remaining bits with pseudo random data
            RandomNumberGenerator.Fill(bytes[8..16]);

            // Calculates next sequence
            // Optionally rewrites the Timestamp if timestamp is not fixed
            CalculateSequence(fixedTimestamp, ref unixTimestampInMs);

            // Set timestamps bits
            SetTimestampBits(unixTimestampInMs, ref bytes);

            // Set sequence bits
            SetSequenceBits(ref bytes);
        }
        else
        {
            // Fill remaining bits with pseudo random data
            RandomNumberGenerator.Fill(bytes[6..16]);

            // Set timestamps bits
            SetTimestampBits(unixTimestampInMs, ref bytes);

        }

        // Set version
        SetVersion(ref bytes);

        // Set variant
        SetVariant(ref bytes);

        // Parse bytes as GUID
        return new Guid(bytes);
    }

    private void CalculateSequence(bool fixedTimestamp, ref long timeStampMs)
    {
        lock (_lockObject)
        {
            if (timeStampMs < _lastTickMs)
                throw new UuidGeneratorException("Clock moved backward");

            if (timeStampMs != _lastTickMs)
            {
                ResetTickSequence(timeStampMs);
                return;
            }

            if (_tickSequence == UuidV7.TickSequenceMaxValue)
            {
                if (fixedTimestamp)
                    throw new UuidGeneratorException("Tick sequence overflow");

                Task.Delay(1);
                timeStampMs = GetUnixTimestampInMs(DateTime.UtcNow);
                ResetTickSequence(timeStampMs);
            }
            else
            {
                ++_tickSequence;
            }
        }
    }

    private void ResetTickSequence(long timeStampMs)
    {
        _lastTickMs = timeStampMs;
        _tickSequence = (short)RandomNumberGenerator.GetInt32(_opts.SequenceStartMinValue, _opts.SequenceStartMaxValue);
    }

    private void SetSequenceBits(ref Span<byte> bytes)
    {
        BinaryPrimitives.WriteInt16LittleEndian(bytes[6..8], _tickSequence);
    }

    private static void SetVersion(ref Span<byte> bytes)
    {
        // Clear version bits
        bytes[v7.UuidV7.VersionOctetLe] &= v7.UuidV7.VersionMask;
        //Set 4 upper bits of version octet to the version
        bytes[v7.UuidV7.VersionOctetLe] |= v7.UuidV7.Version << v7.UuidV7.VersionOffset;
    }

    private static void SetVariant(ref Span<byte> bytes)
    {
        // Clear variant bits
        bytes[v7.UuidV7.VariantOctet] &= v7.UuidV7.VariantMask;
        // Set 2 upper bits of variant octet to the variant
        bytes[v7.UuidV7.VariantOctet] |= v7.UuidV7.Variant << v7.UuidV7.VariantOffset;
    }

    private static long GetUnixTimestampInMs(DateTime utcTimestamp)
    {
        // Calculate unix timestamp
        var unixTimestamp = utcTimestamp - DateTime.UnixEpoch;

        // Convert unix timestamp milliseconds span to long
        return Convert.ToInt64(unixTimestamp.TotalMilliseconds);
    }

    private static void SetTimestampBits(long unixTimestampInMs, ref Span<byte> bytes)
    {
        // Allocate 8 byte space for unix timestamp bits
        Span<byte> unixTimestampBytes = stackalloc byte[8];

        // Convert unix timestamp milliseconds to LittleEndian bytes
        BinaryPrimitives.WriteInt64LittleEndian(unixTimestampBytes, unixTimestampInMs);

        // Set timestamp bits
        unixTimestampBytes[..2].CopyTo(bytes[4..6]);
        unixTimestampBytes[2..6].CopyTo(bytes[..4]);
    }
}