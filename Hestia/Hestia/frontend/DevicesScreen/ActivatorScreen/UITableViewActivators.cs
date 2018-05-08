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
        public Device device;
        public Device Device
        {
            get => device;
            set => device = value;
        }

        public UITableViewActivators (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            Title = device.Name;

            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceActivators(device);
		}
	}
}
