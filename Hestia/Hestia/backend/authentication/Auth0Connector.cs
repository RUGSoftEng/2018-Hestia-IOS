﻿using System.Threading;
using Auth0.OidcClient;
using UIKit;

namespace Hestia.backend.authentication
{
    public class Auth0Connector
    {
        public static Auth0Client CreateAuthClient(UIViewController uIViewController)
        {
            Auth0Client client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "https://hest.io",
                ClientId = Resources.strings.clientId,
                Controller = uIViewController
            });
            return client;
        }
    }
}
