using Foundation;
using System;
using UIKit;

namespace Hestia
{
    public partial class UITableViewControllerServer : UITableViewController
    {
        public UITableViewControllerServer (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            ipLabel.Text = "192.168.0.1";
            serverNameLabel.Text = "Hestia_server2018";

        }
    }
}