using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend
{
    public class Auth0ServerInteractor
    {
        private NetworkHandler networkHandler;

        public Auth0ServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public List<WebServer> GetServers()
        {
            string endpoint = 
        }
    }
}