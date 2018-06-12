using Foundation;
using Hestia.Frontend.Local;
using System;

namespace Hestia.Backend.Utils.ServerDiscovery
{
    public class ServerDelegate : NSNetServiceBrowserDelegate
    {
        public bool Searching { get; set; }
        public NSMutableArray Services { get; set; }
        UITableViewControllerServerDiscovery parent;

        public ServerDelegate(Hestia.Frontend.Local.UITableViewControllerServerDiscovery parent)
        {
            Services = new NSMutableArray(0);
            Searching = false;
            this.parent = parent;
        }

        [Export("netServiceBrowserWillSearch:")]
        public override void SearchStarted(NSNetServiceBrowser sender)
        {
            Console.WriteLine("Started search");
            Searching = true;
            UpdateUI();
        }

        [Export("netServiceBrowserDidStopSearch:")]
        public override void SearchStopped(NSNetServiceBrowser sender)
        {
            Console.WriteLine("Started stopped");
            Searching = false;
            UpdateUI();
        }

        [Export("netServiceBrowser:didNotSearch:")]
        public override void NotSearched(NSNetServiceBrowser sender, NSDictionary errors)
        {
            Searching = false;
            HandleError(errors);
        }

        [Export("netServiceBrowser:didFindService:moreComing:")]
        public override void FoundService(NSNetServiceBrowser sender, 
                                          NSNetService service, bool moreComing)
        {
            Console.WriteLine("Found service " + service.HostName);
            Services.Add(service);
            if(!moreComing)
            {
                UpdateUI();
            }
        }

        [Export("netServiceBrowser:didRemoveService:moreComing:")]
        public override void ServiceRemoved(NSNetServiceBrowser sender, NSNetService service, bool moreComing)
        {
            nint idx = (nint)Services.IndexOf(service);
            Services.RemoveObject(idx);
        }

        public void HandleError(NSDictionary errorDict)
        {
            Console.WriteLine(errorDict.Description);
        }

        public void UpdateUI()
        {
            parent.UpdateServerDiscoveryTable(Services);
        }
    }
}