using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceDevice : UITableViewController
    {
        public Hashtable plugins;
        public UITableViewControllerAddDeviceDevice (IntPtr handle) : base (handle)
        {
        }

    }
}