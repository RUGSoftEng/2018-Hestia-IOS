using Foundation;
using Hestia.Backend.Utils.ServerDiscovery;
using System;
using UIKit;

namespace Hestia.Frontend.Local
{
    public partial class UITableViewControllerServerDiscovery : UITableViewController
    {
        AutoServerDicovery autoServerDiscovery;

        NSMutableArray services;

        public UITableViewControllerServerDiscovery(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            autoServerDiscovery = new AutoServerDicovery(this);
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

		public void UpdateServerDiscoveryTable(NSMutableArray newServices)
        {
            ((TableSourceServerDiscovery)TableView.Source).UpdateServices(newServices);
            TableView.ReloadData();
        }

    }
}
