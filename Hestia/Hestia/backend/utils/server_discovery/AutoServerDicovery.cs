using Foundation;

namespace Hestia.backend.utils.server_discovery
{
    class AutoServerDicovery
    {
        public ServerDelegate Discover()
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