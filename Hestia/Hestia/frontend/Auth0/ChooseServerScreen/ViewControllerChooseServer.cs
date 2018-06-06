using System;
using UIKit;

namespace Hestia
{
    /// <summary>
    /// The ViewController that contains the View that is contains the list with
    /// servers that can be chosen to add a new device to.
    /// See, <see cref="TableSourceAddDeviceChooseServer"/>.
    /// </summary>
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