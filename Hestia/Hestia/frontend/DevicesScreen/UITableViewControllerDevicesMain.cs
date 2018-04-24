using System;
using UIKit;

using System.Collections.Generic;

using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.backend.models;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
        // The table that lives in this view controller
       // public UITableView DevicesTable;

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
            DevicesTable.SetEditing(false, true);
            NavigationItem.RightBarButtonItem = edit;
            ((TableSource)DevicesTable.Source).DidFinishTableEditing(DevicesTable);
        }

        public void setEditingState()
        {
            ((TableSource)DevicesTable.Source).WillBeginTableEditing(DevicesTable);
            DevicesTable.SetEditing(true, true);
            NavigationItem.RightBarButtonItem = done;
        }

        public void refreshDeviceList()
        {
            // Get the list with devices
            try
            {
                devices = Globals.GetDevices();
            }
            catch (ServerInteractionException ex)
            {
                Console.Out.WriteLine("Exception while getting devices from server");
                Console.Out.WriteLine(ex.ToString());
            }
            DevicesTable.Source = new TableSource(devices, this); 
        }

		public override void ViewDidLoad()
        { 
            base.ViewDidLoad();

            // TEMPORARY FOR TESTING
            Globals.LocalLogin = true;
            //=====

            //DevicesTable = new UITableView(View.Bounds); // defaults to Plain style

            refreshDeviceList();

            // To tap row in editing mode for changing name
            DevicesTable.AllowsSelectionDuringEditing = true;
            // Contains methods that describe behavior of table
            DevicesTable.Source = new TableSource(devices, this); 

            // Add the table to the view
            //Add(DevicesTable); 

            // Done button
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                this.cancelEditingState();
            });

            // Edit button
            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                this.setEditingState();
            });

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = edit;
         
        }
    }
}