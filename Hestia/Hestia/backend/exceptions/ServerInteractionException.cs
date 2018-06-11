using System;

namespace Hestia.backend.exceptions
{
    /// <summary>
    /// This exception is the general exception thrown when problems occur during communication with a server.
    /// </summary>
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
