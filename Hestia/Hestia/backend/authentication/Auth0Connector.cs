using System.Threading;
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
                Domain = "hestio.eu.auth0.com",
                ClientId = "_1wsVGXJbkEm8kwcueB5seyxm37E1rWl",
                Controller = uIViewController
            });
            return client;
        }
    }
}
