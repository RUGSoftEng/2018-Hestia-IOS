using Hestia.Frontend;
using Hestia.Frontend.Resources;
using System;
using UIKit;

namespace Hestia.Frontend.SettingsScreen
{
    public partial class UITableViewControllerLocalSettingsScreen : UITableViewController
    {
        public UITableViewControllerLocalSettingsScreen (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// Displays server name on the button for change Server
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            serverName.Text = Globals.ServerName;
        }

        /// <summary>
        /// Reset Defaults button
        /// </summary>
        partial void UIButton87850_TouchUpInside(UIButton sender)
        {
            Globals.ResetAllUserDefaults();
            WarningMessage.Display("User Defaults Reset", "All settings are cleared", this);
        }
    }
}
