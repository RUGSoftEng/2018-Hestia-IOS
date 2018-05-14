using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.DevicesScreen;

namespace Hestia
{
    public partial class UITableViewControllerServerDiscovery : UITableViewController
    {
		//AutoServerDiscovery autoServerDiscovery = new AutoServerDiscovery();

		//NSMutableArray services = autoServerDiscovery.GetServices();

		public UITableViewControllerServerDiscovery (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Contains methods that describe behavior of table
			//TableView.Source = new UITableViewControllerServerDiscovery(services, this);
        }

    }
}
