using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NKelemen18.Uuid;


[Serializable]
[ExcludeFromCodeCoverage]
public class UuidGeneratorException : Exception
{
    public UuidGeneratorException()
    {
    }

    public UuidGeneratorException(string message) : base(message)
    {
    }

    public UuidGeneratorException(string message, Exception exception) : base(message, exception)
    {
    }

    protected UuidGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
