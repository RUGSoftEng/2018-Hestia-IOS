using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.DevicesScreen;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceDevice : UITableViewController
    {
        public List<string> plugins;
        public string collection;

        public UITableViewControllerAddDeviceDevice (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Contains methods that describe behavior of table
            Devices.Source = new TableSourceAddDeviceDevice(plugins, collection, this);
        }

    }
}
