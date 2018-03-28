using Foundation;
using ObjCRuntime;
using System;
using UIKit;

using Hestia.DevicesScreen;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia
{


    public partial class UITableViewControllerServerConnect : UITableViewController
    {

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            
        }
        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            newServerName.Placeholder = "Hestia_Server2018";
            newIP.Placeholder = "94.212.164.28";
            newPort.Placeholder = "8000";


        }
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);


            //Set up Destination View Controller
            if (segue.Identifier == "ServerToDevice")
            {
                var DevicesController = (UITableViewControllerDevicesMain)segue.DestinationViewController;
                if (DevicesController != null)
                {
                }
            }

        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected (tableView, indexPath);

            Console.WriteLine(indexPath.Section);
            if (indexPath.Section == 1 && indexPath.Row == 0)
            {
                PerformSegue("ServerToDevices", this);
            }
        }
    }
}