using System;
using System.Net;

namespace Accountancy.Infrastructure.Exceptions
{
    public abstract class KnownException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        protected KnownException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}