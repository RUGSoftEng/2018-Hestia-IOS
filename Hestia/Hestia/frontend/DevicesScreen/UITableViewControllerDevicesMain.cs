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
       // Done button in top right (appears in edit mode)
        UIBarButtonItem done;
        // Edit button in top right (is shown initially)
        UIBarButtonItem edit;

        List<Device> devices = new List<Device>();

        // Constructor
        public UITableViewControllerDevicesMain(IntPtr handle) : base(handle)
        {
        }

        public void CancelEditingState()
        {
            DevicesTable.SetEditing(false, true);
            NavigationItem.RightBarButtonItem = edit;
            ((TableSource)DevicesTable.Source).DidFinishTableEditing(DevicesTable);
        }

        public void SetEditingState()
        {
            ((TableSource)DevicesTable.Source).WillBeginTableEditing(DevicesTable);
            DevicesTable.SetEditing(true, true);
            NavigationItem.RightBarButtonItem = done;
        }

        public void RefreshDeviceList()
        {
            // Get the list with devices
            if (Globals.LocalLogin)
            {
                try
                {
                    devices = Globals.GetDevices();
                }
                catch (ServerInteractionException ex)
                {
                    Console.Out.WriteLine("Exception while getting devices from server");
                    Console.Out.WriteLine(ex.ToString());
                }
            }
            // In case of Global login, devices are fetch in construtor of TableSource
            DevicesTable.Source = new TableSource(devices, this); 
        }

		public override void ViewDidLoad()
        { 
            base.ViewDidLoad();
            RefreshDeviceList();

            // To tap row in editing mode for changing name
            DevicesTable.AllowsSelectionDuringEditing = true;  

            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                CancelEditingState();
            });

            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                SetEditingState();
            });

            //Pull to refresh
            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += RefreshTable;
            TableView.Add(RefreshControl);

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = edit;
        }

        //Method Pull to refresh
        private void RefreshTable(object sender, EventArgs e)
        {
            RefreshControl.BeginRefreshing();
            RefreshControl.EndRefreshing();
            TableView.ReloadData();
        }
    }
}
