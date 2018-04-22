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


namespace Hestia
{
    public partial class UITableViewControllerAddDeviceChooseServer : UITableViewController
    {
        // The table that lives in this view controller
        UITableView table;

        public UITableViewControllerAddDeviceChooseServer(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style

      
            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceChooseServer(this);

            // Add the table to the view
            Add(table);

        }
    }
}