using System;
using System.Collections.Generic;
using UIKit;
using Hestia.DevicesScreen;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceDevice : UITableViewController
    {
        public List<string> plugins;
        public string collection;

        public UITableViewControllerAddDeviceDevice (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Contains methods that describe behavior of table
            Devices.Source = new TableSourceAddDeviceDevice(plugins, collection, this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            // Cancel button to go back to Devices main screen
            UIBarButtonItem cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) =>
            {
                NavigationController.PopToRootViewController(true);
            });
            NavigationItem.RightBarButtonItem = cancel;
        }
    }
}
