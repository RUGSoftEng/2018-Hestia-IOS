using Foundation;
using System;
using UIKit;

using Hestia.DevicesScreen.resources;

namespace Hestia.DevicesScreen
{
    public partial class SettingsController : UITableViewController
    {
        public SettingsController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            serverName.Text = Globals.ServerName;
            userName.Text = Globals.UserName;
        }
    }

}