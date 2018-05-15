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

        // Reset Defaults button
		partial void UIButton87850_TouchUpInside(UIButton sender)
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