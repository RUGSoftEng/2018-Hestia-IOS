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
        partial void UIButton89405_TouchUpInside(UIButton sender)
        {
            Globals.ResetAllUserDefaults();
            WarningMessage.Display("User Defaults Reset", "All settings are cleared", this);
        }
    }
}
