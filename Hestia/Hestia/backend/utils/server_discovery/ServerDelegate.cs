﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend.utils.server_discovery
{
    public class ServerDelegate : NSNetServiceBrowserDelegate
    {
        public bool Searching { get; set; }
        public NSMutableArray Services { get; set; }

        public ServerDelegate()
        {
            Services = new NSMutableArray(0);
            Searching = false;
        }

        public void NetServiceBrowserWillSearch(NSNetServiceBrowser browser)
        {
            Searching = true;
            UpdateUI();
        }

        public void NetServiceBrowserDidStopSearch(NSNetServiceBrowser browser)
        {
            Searching = false;
            UpdateUI();
        }

        public void NetServiceBrowser(NSNetServiceBrowser browser, NSDictionary errorDict)
        {
            Searching = false;
            HandleError(errorDict);
        }



        public void HandleError(NSDictionary errorDict)
        {

        }

        public void UpdateUI()
        {

        }
    }
}