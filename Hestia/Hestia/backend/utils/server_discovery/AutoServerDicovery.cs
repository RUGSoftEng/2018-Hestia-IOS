using Foundation;

namespace Hestia.backend.utils.server_discovery
{
    /// <summary>
    /// This class should be used to get the display the list of servers that are found by the service.
    /// The delegate contains a list of services that are continuously update by this class untill Stop is called.
    /// </summary>
    public class AutoServerDicovery
    {
        public NSNetServiceBrowser Browser { get; set; }
        public ServerDelegate Delegate { get; set; }

        public AutoServerDicovery()
        {
            // Setup of the service browser.
            Browser = new NSNetServiceBrowser();
            Delegate = new ServerDelegate();
            Browser.Init();
            Browser.Delegate = Delegate;            
        }

        public void Discover()
        {
            Browser.SearchForServices("_hestia._tcp", @"local");
        }

        public void Stop()
        {
            Browser.Stop();
        }
    }
}