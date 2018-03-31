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
    public partial class UITableViewControllerAddDeviceProperties : UITableViewController
    {
        UITableView table;

        // The table that lives in this view controller


        public List<string> properties;

        public UITableViewControllerAddDeviceProperties(IntPtr handle) : base(handle)
        {
        }



        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style


            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceProperties(properties, this);

          
            // Add the table to the view
            Add(table);

        }

    }
}