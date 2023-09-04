using System.Buffers.Binary;

namespace NKelemen18.Uuid.v7;

public static class UuidV7Decoder
{
    public static (DateTime, short) Decode(string guid)
    {
        return Decode(Guid.Parse(guid));
    }

    public static (DateTime, short) Decode(Guid guid)
    {
        Span<byte> bytes = stackalloc byte[16];
        guid.TryWriteBytes(bytes);

        EnsureVersionIsCorrect(ref bytes);
        EnsureVariantIsCorrect(ref bytes);
        var unixTimestampInMs = GetTimestampInMs(ref bytes);
        var utcDateTime = GetTimestampUtcDateTime(unixTimestampInMs);
        var sequence = GetSequence(ref bytes);
        return (utcDateTime, sequence);
    }

    private static long GetTimestampInMs(ref Span<byte> bytes)
    {
        // Allocate 8 byte space for unix timestamp bits
        Span<byte> unixTimestampBytes = stackalloc byte[8];

        // Copy first 6 bytes of guid which contains the timestamp
        bytes[4..6].CopyTo(unixTimestampBytes);
        bytes[..4].CopyTo(unixTimestampBytes[2..6]);

        return BinaryPrimitives.ReadInt64LittleEndian(unixTimestampBytes);
    }

    private static void EnsureVersionIsCorrect(ref Span<byte> bytes)
    {
        var versionOctet = bytes[UuidV7.VersionOctetLe];
        if (UuidV7.Version != versionOctet >> UuidV7.VersionOffset)
            throw new UuidDecoderException("Version is incorrect");
    }

    private static void EnsureVariantIsCorrect(ref Span<byte> bytes)
    {
        var variantOctet = bytes[UuidV7.VariantOctet];
        if (UuidV7.Variant != variantOctet >> UuidV7.VariantOffset)
            throw new UuidDecoderException("Variant is incorrect");
    }

    private static DateTime GetTimestampUtcDateTime(long unixTimestampInMs)
    {
        return DateTime.UnixEpoch.AddMilliseconds(unixTimestampInMs).ToUniversalTime();
    }

    private static short GetSequence(ref Span<byte> bytes)
    {
        var sequenceBytes = bytes[6..8];
        sequenceBytes[1] &= UuidV7.VersionMask;
        return BinaryPrimitives.ReadInt16LittleEndian(sequenceBytes);
    }
}