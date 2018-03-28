using Foundation;
using ObjCRuntime;
using System;
using UIKit;

using Hestia;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen
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
            newIP.Text = "94.212.164.28";
            newPort.Text = "8000";


        }
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);


            //Set up Destination View Controller
            if (segue.Identifier == "ServerToDevice")
            {
                var devicesController = (UITableViewControllerDevicesMain)segue.DestinationViewController;
                if (devicesController != null)
                {
                    ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler("94.212.164.28", 8000));
                    //devicesController.ServerInteractor = serverInteractor;
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