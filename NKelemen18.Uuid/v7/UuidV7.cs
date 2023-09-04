namespace NKelemen18.Uuid.v7;

public static class UuidV7
{
    public const short TickSequenceMaxValue = 4096;
    public const byte Version = 7;
    public const byte Variant = 2;
    internal const short VersionOctetBe = 6; // Big endian
    internal const short VersionOctetLe = 7; // Low endian
    internal const short VariantOctet = 8;
    internal const short VersionOffset = 4;
    internal const short VariantOffset = 6;
    internal const byte VersionMask = 0b0000_1111;
    internal const byte VariantMask = 0b0011_1111;

    private static readonly UuidV7Generator Generator = new();
    public static Guid NewUuidV7() => Generator.NewUuidV7();
    public static (DateTime, short) Decode(Guid guid) => UuidV7Decoder.Decode(guid);
    public static (DateTime, short) Decode(string guid) => UuidV7Decoder.Decode(guid);
}