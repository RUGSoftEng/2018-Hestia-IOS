using Auth0.OidcClient;
using UIKit;

namespace Hestia.Backend.Authentication
{
    public static class Auth0Connector
    {
        public static Auth0Client CreateAuthClient(UIViewController uIViewController)
        {
            Auth0Client client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = Resources.strings.domainAuth0,
                ClientId = Resources.strings.clientId,
                Controller = uIViewController
            });
            return client;
        }
    }
}
