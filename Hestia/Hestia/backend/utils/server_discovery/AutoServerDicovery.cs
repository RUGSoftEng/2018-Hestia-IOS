using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend.utils.server_discovery
{
    class AutoServerDicovery
    {
        public ServerInteractor Discover()
        {
            // Setup of the service browser.
            NSNetServiceBrowser browser = new NSNetServiceBrowser();
            NSNetServiceDelegate @delegate = new NSNetServiceDelegate();
            browser.Init();
            browser.Delegate = (INSNetServiceBrowserDelegate) @delegate;
            
            browser.SearchForServices("_hestia._tcp", @"local");
        }
    }
}