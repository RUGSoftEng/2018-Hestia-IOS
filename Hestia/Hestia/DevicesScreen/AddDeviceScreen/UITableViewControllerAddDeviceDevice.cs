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
        // The table that lives in this view controller
        UITableView table;

        public List<string> plugins;
        public string collection;

        public UITableViewControllerAddDeviceDevice (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style

            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceDevice(plugins, collection, this);

            // Add the table to the view
            Add(table);

        }

    }
}