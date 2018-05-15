using System;

namespace Hestia.backend.exceptions
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