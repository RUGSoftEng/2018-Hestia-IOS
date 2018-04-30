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
        
        public UITableViewControllerAddDeviceChooseServer(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
               
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceAddDeviceChooseServer(this);
        }
    }
}