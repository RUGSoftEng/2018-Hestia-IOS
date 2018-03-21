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
    class ComFaultException : Exception
    {
        private String error;
        private String message;

        public String Error { get; set; }
        new
        public String Message { get; set; }

        public ComFaultException(String error)
        {
            this.error = error;
            this.message = new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("errorMessage");
        }
    }
}