using Foundation;
using Hestia.Auth0;
using System;
using UIKit;

namespace Hestia
{
    public partial class UITableViewControllerChooseServer : UITableViewController
    {
        public UITableViewControllerChooseServer (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceAddDeviceChooseServer(this);
		}
	}
}