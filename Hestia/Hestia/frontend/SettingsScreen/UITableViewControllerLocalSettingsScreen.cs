using Foundation;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;
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

        // Reset Defaults button
        partial void UIButton87850_TouchUpInside(UIButton sender)
        {
            Globals.ResetAllUserDefaults();
            new WarningMessage("User Defaults Reset", "All settings are cleared", this);
        }
    }
}