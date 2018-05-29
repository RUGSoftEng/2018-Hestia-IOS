using System;
using UIKit;

namespace Hestia.Auth0
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