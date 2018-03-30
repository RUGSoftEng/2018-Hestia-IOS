﻿using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen
{
    public class TableSourceAddDeviceManufacturer : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDevice owner;

        Hashtable plugins;

        // The list with Devices, set in the constructor. (Retrieved from server)
        List<string> collections;

        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "tableCell";

        // Constructor. Gets the device data and the ViewController
        public TableSourceAddDeviceManufacturer(List<string> collections,
                                    Hashtable plugins,
                    UITableViewControllerAddDevice owner)
        {
            this.plugins = plugins;
            this.owner = owner;
            this.collections = collections;
        }

        // We have only one section with devices (thus far)
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        // The number of manufacturers in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return collections.Count;
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

            // The text to display on the cell is the device name
            cell.TextLabel.Text = collections[indexPath.Row];

            return cell;

        }


        // Devices what happens if touch on row.
        // Should display the slider(s) ultimately
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
           // UIAlertController okAlertController = UIAlertController.Create("Row Selected", TableItems[indexPath.Row].Name, UIAlertControllerStyle.Alert);
            //okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

           // owner.PresentViewController(okAlertController, true, null);

           // tableView.DeselectRow(indexPath, true);

            // Launches a new instance of CallHistoryController
            UITableViewControllerAddDeviceDevice addDeviceType = this.owner.Storyboard.InstantiateViewController("AddDevice") as UITableViewControllerAddDeviceDevice;
            if (addDeviceType != null)
            {
                addDeviceType.plugins = this.plugins;
                this.owner.NavigationController.PushViewController(addDeviceType, true);
            }
        

        }

    }
}