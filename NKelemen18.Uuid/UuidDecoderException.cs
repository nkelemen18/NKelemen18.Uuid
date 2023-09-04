using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace NKelemen18.Uuid;

[Serializable]
[ExcludeFromCodeCoverage]
public class UuidDecoderException : Exception
{
    public UuidDecoderException()
    {
    }

    public UuidDecoderException(string message) : base(message)
    {
    }

    public UuidDecoderException(string message, Exception exception) : base(message, exception)
    {
    }

    protected UuidDecoderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}