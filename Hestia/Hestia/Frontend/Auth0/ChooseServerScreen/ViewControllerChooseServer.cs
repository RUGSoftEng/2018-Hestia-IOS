using System;
using UIKit;

namespace Hestia.Frontend.Auth0.ChooseServerScreen
{
    /// <summary>
    /// The ViewController that holds the View that contains the list with
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