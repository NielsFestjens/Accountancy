using System;

namespace Accountancy.Infrastructure.Exceptions
{
    public abstract class KnownException : Exception
    {
        public KnownException(string message) : base(message)
        {
        }
    }
}