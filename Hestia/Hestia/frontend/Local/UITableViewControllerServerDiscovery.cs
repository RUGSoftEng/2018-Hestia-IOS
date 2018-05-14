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
using Hestia.backend.utils.server_discovery;

namespace Hestia
{
    public partial class UITableViewControllerServerDiscovery : UITableViewController
    {
        AutoServerDicovery autoServerDiscovery = new AutoServerDicovery();

        NSMutableArray services;

        public UITableViewControllerServerDiscovery(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            services = autoServerDiscovery.GetServices();
            int count = (int)services.Count;
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceServerDiscovery(services, this);
        }

		public override void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);
            autoServerDiscovery.Search();
		}

		public override void ViewDidDisappear(bool animated)
		{
            base.ViewDidDisappear(animated);
            autoServerDiscovery.Stop();
		}

		public void updateServerDiscoveryTable(NSMutableArray newServices)
        {
            ((TableSourceServerDiscovery)TableView.Source).UpdateServices(newServices);
            TableView.ReloadData();
        }

    }
}
