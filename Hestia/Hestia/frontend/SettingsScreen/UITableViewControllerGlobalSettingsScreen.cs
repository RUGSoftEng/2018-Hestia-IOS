using Foundation;
using System;
using UIKit;
using Hestia.DevicesScreen.resources;

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
            DisplayConfirmationMessage();
        }

        private void DisplayConfirmationMessage()
        {
            string title = "User Defaults Reset";
            string message = "All settings are cleared";
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
        }
    }
}