using Foundation;
using Hestia.DevicesScreen.resources;
using System;
using UIKit;

namespace Hestia
{
    public partial class UITableViewControllerLocalSettingsScreen : UITableViewController
    {
        public UITableViewControllerLocalSettingsScreen (IntPtr handle) : base (handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            serverName.Text = Globals.ServerName;
        }
	}
}