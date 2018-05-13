using Foundation;

namespace Hestia.backend.utils.server_discovery
{
    /// <summary>
    /// This class should be used to get the display the list of servers that are found by the service.
    /// The delegate contains a list of services that are continuously update by this class untill Stop is called.
    /// </summary>
    public class AutoServerDicovery
    {
        public NSNetServiceBrowser Browser { get; }
        public ServerDelegate Delegate { get; }
        private const string PROTOCOL = "_hestia._tcp";
        private const string DOMAIN = @"local";

        public AutoServerDicovery()
        {
            // Setup of the service browser.
            Browser = new NSNetServiceBrowser();
            Delegate = new ServerDelegate();
            Browser.Init();
            Browser.Delegate = Delegate;            
        }

        public void Search()
        {
            Browser.SearchForServices(PROTOCOL, DOMAIN);
        }

        public void Stop()
        {
            Browser.Stop();
        }

        public NSMutableArray GetServices()
        {
            return Delegate.Services;
        }
    }
}