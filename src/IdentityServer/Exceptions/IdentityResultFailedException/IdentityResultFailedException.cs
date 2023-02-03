using System.Runtime.Serialization;

namespace IdentityServer.Exceptions.IdentityResultFailedException;

public class IdentityResultFailedException : Exception
{
    public string? Code {get;}
    public IdentityResultFailedException(string code)
    {
        Code = code;
    }

    public IdentityResultFailedException(string? message, string code) : base(message)
    {
        Code = code;
    }

    public IdentityResultFailedException(string? message, Exception? innerException, string code) : base(message, innerException)
    {
        Code = code;
    }

    protected IdentityResultFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}