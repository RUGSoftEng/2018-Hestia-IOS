using Foundation;
using System;
using UIKit;

using System.Drawing; 
using System.Collections.Generic;
using System.Collections;

using Hestia.DevicesScreen;
using Hestia.backend;
using Hestia.backend.models;


namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
        ServerInteractor serverInteractor;
        //public ServerInteractor ServerInteractor { get; set; }


        // The table that lives in this view controller
        UITableView table;


        // Done button in top right (appears in edit mode)
        UIBarButtonItem done;
        // Edit button in top right (is shown initially)
        UIBarButtonItem edit;

        List<Device> devices = new List<Device>();

        // Constructor
        public UITableViewControllerDevicesMain(IntPtr handle) : base(handle)
        {
            serverInteractor = new ServerInteractor(new NetworkHandler("94.212.164.28", 8000));
        }

        public override void ViewDidLoad()
        {
            
            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style

            // The list with devices
            try
            {
                devices = serverInteractor.GetDevices();
            }
            catch(Exception e){
                Console.Out.WriteLine(e.StackTrace);
            }
            // Contains methods that describe behavior of table
            table.Source = new TableSource(devices, this); 
            //table.Source = new TableSource(tableItems, this); 
            // Add the table to the view
            Add(table); 


            // Done button
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                table.SetEditing(false, true);
                NavigationItem.RightBarButtonItem = edit;
                ((TableSource)table.Source).DidFinishTableEditing(table);
            });

            // Edit button
            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                if (table.Editing)
                    table.SetEditing(false, true); // if we've half-swiped a row
                ((TableSource)table.Source).WillBeginTableEditing(table);
                table.SetEditing(true, true);
                NavigationItem.RightBarButtonItem = done;
            });

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = edit;
         
        }
    }
}