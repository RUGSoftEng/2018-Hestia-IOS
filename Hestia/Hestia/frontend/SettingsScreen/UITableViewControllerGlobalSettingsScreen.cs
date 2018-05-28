using Foundation;
using System;
using UIKit;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;

namespace Hestia
{
    public partial class UITableViewControllerGlobalSettingsScreen : UITableViewController
    {
        public UITableViewControllerGlobalSettingsScreen (IntPtr handle) : base (handle)
        {
        }

        // Reset Defaults button
        partial void ResetDefaultsButton_TouchUpInside(UIButton sender)
        {
            Globals.ResetAllUserDefaults();
            new WarningMessage("User Defaults Reset", "All settings are cleared", this);
        }
    }
}