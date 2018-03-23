using Foundation;
using System;
using UIKit;

using System.Drawing; 
using System.Collections.Generic;
using System.Collections;

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
            //string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };

            List<DataItem> tableItems = new List<DataItem>();
            tableItems.Add(new DataItem(){ Label = " Device 1", State = true });
            tableItems.Add(new DataItem(){ Label = " Device 2", State = false });


            table.Source = new TableSource(tableItems, this); // contains behavior of table
            Add(table); // Add the table to the view



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

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = edit;
          


        }
    }
}