﻿using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.DevicesScreen.EditDevice;

namespace Hestia.DevicesScreen
{
    public class TableSource : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerDevicesMain owner;

        // Use a Hashtable to hold the switches - this means that when a switch is 
        //created for a row which may have previously been displayed, 
        //we know we're replacing the old one because the Hashtable is keyed on row.
        Hashtable Switches = new Hashtable();

        // The list with Devices, set in the constructor. (Retrieved from server)
        List<Device> TableItems;

        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "tableCell";

        // Constructor. Gets the device data and the ViewController
        public TableSource(List<Device> items, UITableViewControllerDevicesMain owner)
        {
            TableItems = items;
            this.owner = owner;
        }

        // We have only one section with devices (thus far)
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        // The number of devices in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

        // Important method. Called to generate a cell to display
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }


            // Create a new UISwitch and set up its delegate for the value changing
            foreach (Hestia.backend.models.Activator act in TableItems[(indexPath.Row)].Activators)
            {
                if (act.State.Type == "bool" && (act.Name == "On/Off" || act.Name == "Activate"))
                {
                    UISwitch DeviceSwitch = new UISwitch();
                    // Set the switch's state to that of the device.
                    DeviceSwitch.On = (bool)act.State.RawState;
                    DeviceSwitch.ValueChanged += delegate (object sender, EventArgs e)
                    {
                        act.State = new Hestia.backend.models.ActivatorState<object>(DeviceSwitch.On, "bool");
                    };
                    // Replace the cell's AccessoryView with the new UISwitch
                    cell.AccessoryView = DeviceSwitch;

                    // Keep a reference to the UISwitch - note using a Hashtable to ensure
                    // we only have one for any given row
                    Switches[indexPath.Row] = cell.AccessoryView;

                    cell.EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
                }
            }

                // The text to display on the cell is the device name
                cell.TextLabel.Text = TableItems[(indexPath.Row)].Name;

                return cell;
            
        }


        // Devices what happens if touch on row.
        // Should display the slider(s) ultimately
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (!tableView.Editing)
            {
                UIAlertController okAlertController = UIAlertController.Create("Row Selected", TableItems[indexPath.Row].Name, UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                owner.PresentViewController(okAlertController, true, null);
                tableView.DeselectRow(indexPath, true);
            }
            else if(tableView.Editing)
            {

                UIViewControllerEditDeviceName editViewController = new UIViewControllerEditDeviceName();
                 this.owner.NavigationController.PushViewController(editViewController, true);

          

                //UIAlertController okAlertController = UIAlertController.Create("Editing Row Selected", TableItems[indexPath.Row].Name, UIAlertControllerStyle.Alert);
               // okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
               // owner.PresentViewController(okAlertController, true, null);
                tableView.DeselectRow(indexPath, true);
            }
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    try
                    {
                        // remove device from server 
                        Globals.ServerInteractor.RemoveDevice(TableItems[indexPath.Row]);
                    }
                    catch(Exception e)
                    {
                        Console.Out.WriteLine(e.StackTrace);
                    }
                    TableItems.RemoveAt(indexPath.Row);
                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
                case UITableViewCellEditingStyle.Insert:
                    UITableViewControllerAddDevice addDeviceVc = 
                        this.owner.Storyboard.InstantiateViewController("AddManufacturer") 
                             as UITableViewControllerAddDevice;
                    owner.NavigationController.PushViewController(addDeviceVc, true);
                    break;
            }
        }

        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {   // Optional - default text is 'Delete'
            return "Remove " + TableItems[indexPath.Row].Name;
        }


        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                return true; // return false if you wish to disable editing for a specific indexPath or for all rows
            }
            else
            {
                return false;
            }
        }


        // Defines the red delete/add buttons before cell
        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                // Add new device below table
                if (indexPath.Row == tableView.NumberOfRowsInSection(0) - 1)
                    return UITableViewCellEditingStyle.Insert;
                else
                    return UITableViewCellEditingStyle.Delete;
            }
            else // not in editing mode, enable swipe-to-delete for all rows
                return UITableViewCellEditingStyle.Delete;
            // This above should change to pop-up for delete confirmation
        }

        // Is called after a press on edit button
        public void WillBeginTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();

            // insert the 'ADD NEW' row at the end of table display
            tableView.InsertRows(new NSIndexPath[] {
                 NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
                 }, UITableViewRowAnimation.Fade);

            // create a new item and add it to our underlying data 
            // This should not be permanently stored, but trigger the add new
            // device screen on touch
            List<backend.models.Activator> temp_activator = new List<backend.models.Activator>();
            NetworkHandler temp_networkhandler = new NetworkHandler("94.212.164.28", 8000);
            TableItems.Add(new Device(" ", "New Device ", " ", temp_activator, temp_networkhandler));

            tableView.EndUpdates(); // applies the changes
        }
        public void DidFinishTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();

            // remove our 'New device' row from the underlying data
            TableItems.RemoveAt((int)tableView.NumberOfRowsInSection(0) - 1); // zero based :)
                                                                              // remove the row from the table display
            tableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(tableView.NumberOfRowsInSection(0) - 1, 0) }, UITableViewRowAnimation.Fade);

            tableView.EndUpdates(); // applies the changes
        }

    }
}
