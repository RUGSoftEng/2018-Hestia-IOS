using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.utils;

namespace Hestia
{
    public partial class UITableViewServerList : UITableViewController
    {
        UIBarButtonItem done;

        public UITableViewServerList (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad(); 
            TableView.Source = new TableSourceServerList();

            // The done button that loads the devicesMainScreen with the selected servers
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                UIStoryboard devicesMainStoryboard = UIStoryboard.FromName("Devices2", null);

                var devicesMain = devicesMainStoryboard.InstantiateViewController("navigationDevicesMain") as UINavigationController;
                ShowViewController(devicesMain, this);
            });
            NavigationItem.RightBarButtonItem = done;
        }
    }
}
