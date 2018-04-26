using System;
using UIKit;

using System.Collections.Generic;

using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.backend.models;

namespace Hestia
{
    public partial class UITableViewControllerServerDevice : UITableViewController
    {
     
        public UITableViewControllerServerDevice (IntPtr handle) : base (handle)
        {
        }


       
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.Source = new TableSourceServerDevices(this);
         

        }
    }
}