using Foundation;
using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewActivators : UITableViewController
    {
        //public UITableView TableView;
        public Device device;
        public Device Device { get; set; }

        public UITableViewActivators (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            Title = device.Name;

            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceActivators(device);
           

            // Add the table to the view
            //Add(TableView); 

		}
	}
}