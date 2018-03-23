using Foundation;
using System;
using UIKit;
using Hestia.DevicesScreen;

namespace Hestia
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
        UITableView table;
        UIBarButtonItem done;
        UIBarButtonItem edit;
      
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



            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                table.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = edit;
                ((TableSource)table.Source).DidFinishTableEditing(table);
            });
            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                if (table.Editing)
                    table.SetEditing(false, true); // if we've half-swiped a row
                ((TableSource)table.Source).WillBeginTableEditing(table);
                table.SetEditing(true, true);
                NavigationItem.LeftBarButtonItem = null;
                NavigationItem.RightBarButtonItem = done;
            });

            NavigationItem.RightBarButtonItem = edit;
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Play);


        }
    }
}