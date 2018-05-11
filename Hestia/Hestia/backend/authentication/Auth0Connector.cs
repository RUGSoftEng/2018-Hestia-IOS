﻿using System.Threading;
using Auth0.OidcClient;
using UIKit;

namespace Hestia.backend.authentication
{
    public class Auth0Connector
    {
        private const string SCOPE = "openid";

        public static Auth0Client CreateAuthClient(UIViewController uIViewController)
        {
            Auth0Client client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = Resources.strings.domainAuth0,
                ClientId = Resources.strings.clientId,
                Controller = uIViewController,
                Scope = SCOPE
            });
            return client;
        }
    }
}