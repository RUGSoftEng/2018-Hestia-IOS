using Hestia.Frontend.Resources;
using System;
using UIKit;

namespace Hestia.Frontend.SettingsScreen
{
    public partial class UITableViewControllerGlobalSettingsScreen : UITableViewController
    {
        public UITableViewControllerGlobalSettingsScreen (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        ///  Reset Defaults button
        /// </summary>
        partial void UIButton89405_TouchUpInside(UIButton sender)
        {
            Globals.ResetAllUserDefaults();
            WarningMessage.Display("User Defaults Reset", "All settings are cleared", this);
        }
    }
}
