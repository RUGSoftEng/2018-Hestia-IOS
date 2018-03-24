using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend.exceptions
{
    class ServerInteractionException : Exception
    {
        public ServerInteractionException()
        {
        }

        public ServerInteractionException(string message)
            : base(message)
        {
        }
    }
}