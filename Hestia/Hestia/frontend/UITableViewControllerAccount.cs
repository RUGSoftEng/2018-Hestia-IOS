using Foundation;
using System;
using UIKit;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerAccount : UITableViewController
    {
        public UITableViewControllerAccount (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            nameLabel.Text = "Tomandrei OancaBoon";
            mailLabel.Text = "hestia@rug.nl";
            phoneLabel.Text = "0612757999";

        }
    }
}