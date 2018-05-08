using Foundation;

namespace Hestia.backend.utils.server_discovery
{
    public class AutoServerDicovery
    {
        public static ServerDelegate Discover()
        {
            // Setup of the service browser.
            NSNetServiceBrowser browser = new NSNetServiceBrowser();
            ServerDelegate @delegate = new ServerDelegate();
            browser.Init();
            browser.Delegate = @delegate;

            browser.SearchForServices("_hestia._tcp", @"local");
            return @delegate;
        }
    }
}