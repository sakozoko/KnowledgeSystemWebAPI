using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace UserService.Exceptions.IdentityResultFailedException;

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