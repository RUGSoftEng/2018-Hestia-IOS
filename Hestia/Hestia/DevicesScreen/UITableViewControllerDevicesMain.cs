using Foundation;
using System;
using UIKit;

using System.Drawing; 
using System.Collections.Generic;
using System.Collections;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.models;


namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
        // The table that lives in this view controller
        public UITableView table;

        // Done button in top right (appears in edit mode)
        UIBarButtonItem done;
        // Edit button in top right (is shown initially)
        UIBarButtonItem edit;

        List<Device> devices = new List<Device>();

        // Constructor
        public UITableViewControllerDevicesMain(IntPtr handle) : base(handle)
        {
        }

        public void cancelEditingState()
        {
            table.SetEditing(false, true);
            NavigationItem.RightBarButtonItem = edit;
            ((TableSource)table.Source).DidFinishTableEditing(table);
        }

		public override void ViewDidLoad()
        { 
            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style

            // Get the list with devices
            try
            {
                devices = Globals.ServerInteractor.GetDevices();
            }
            catch(Exception e){
                Console.Out.WriteLine("Exception while getting devices from server");
                Console.Out.WriteLine(e.StackTrace);
            }

            // To tap row in editing mode for changing name
            table.AllowsSelectionDuringEditing = true;
            // Contains methods that describe behavior of table
            table.Source = new TableSource(devices, this); 

            // Add the table to the view
            Add(table); 

            // Done button
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                this.cancelEditingState();
            });

            // Edit button
            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                ((TableSource)table.Source).WillBeginTableEditing(table);
                table.SetEditing(true, true);
                NavigationItem.RightBarButtonItem = done;
            });

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = edit;
         
        }
    }
}