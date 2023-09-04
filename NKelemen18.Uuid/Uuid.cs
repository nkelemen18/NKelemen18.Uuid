using System.Diagnostics.CodeAnalysis;

namespace NKelemen18.Uuid;

public static class Uuid
{
    public static Guid Nil => Guid.Empty;
    public static Guid Max => new("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
    
    [ExcludeFromCodeCoverage]
    public static Guid NewUuidV4() => Guid.NewGuid();

    [ExcludeFromCodeCoverage]
    public static Guid NewUuidV7() => v7.UuidV7.NewUuidV7();

}