using UIKit;
using Foundation;
using System.Collections;
using System;

using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.AddDeviceScreen;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Hestia
{
    public partial class AddServerViewController : UITableViewController
    {
        UIBarButtonItem done;
        public Hashtable inputFields = new Hashtable();
        public List<string> finalInfo;
        HestiaWebServerInteractor hestiaWebServerInteractor;
        public AddServerViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            Title = "New server";
            List<string> requiredinfo = new List<string> ();
            requiredinfo.Add("name");
            requiredinfo.Add("address");
            requiredinfo.Add("port");
            View.BackgroundColor = Globals.DefaultLightGray;
            TableView.Source = new TableSourceAddServer(this);
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
            {
                var i = 0;
                foreach(string info in requiredinfo)
                {
                    finalInfo[i] = ((PropertyCell)inputFields[info]).inputField.Text;
                }

                hestiaWebServerInteractor.AddServer(finalInfo[0], finalInfo[1], int.Parse(finalInfo[2]));
                SegueToDevicesMain();
            });

            NavigationItem.RightBarButtonItem = done;
		}

        void SegueToDevicesMain()
        {
            // Get the root view contoller and cancel the editing state
            var rootViewController = NavigationController.ViewControllers[0] as UITableViewControllerDevicesMain;
            rootViewController.CancelEditingState();
            rootViewController.RefreshDeviceList();
            // Go back to the devices main screen
            NavigationController.PopToViewController(rootViewController, true);
        }
	}
}