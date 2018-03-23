using Foundation;
using System;
using UIKit;
using Hestia.DevicesScreen;

namespace Hestia
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
        UITableView table;
        public UITableViewControllerDevicesMain (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style
            string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            table.Source = new TableSource(tableItems, this);
            Add(table);
        }
    }
}