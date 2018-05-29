using Foundation;
using System;
using UIKit;

namespace Hestia
{
    public partial class ViewControllerChooseServer : UITableViewController
    {
        public ViewControllerChooseServer(IntPtr handle) : base(handle)
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