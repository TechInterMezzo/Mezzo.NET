using System;

namespace Mezzo.Interop
{
    public class NullPointerException : NullReferenceException
    {
        public NullPointerException()
        { }

        public NullPointerException(string? message)
            : base(message)
        { }

        public NullPointerException(string? message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
