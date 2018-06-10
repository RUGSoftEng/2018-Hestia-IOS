using System;

namespace Hestia.Backend.Exceptions
{
    public class ServerInteractionException : Exception
    {
        public ServerInteractionException()
        {
        }

        public ServerInteractionException(string message)
            : base(message)
        {
        }

        public ServerInteractionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}