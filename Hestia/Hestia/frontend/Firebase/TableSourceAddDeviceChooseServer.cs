using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia
{
    public class TableSourceAddDeviceChooseServer : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceChooseServer owner;

    // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "servercell";

        // Constructor. Gets the device data and the ViewController
        public TableSourceAddDeviceChooseServer(UITableViewControllerAddDeviceChooseServer owner)
        {
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
            return Globals.FirebaseServerInteractors.Count;
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

            // The text to display on the cell is the plugin name
            cell.TextLabel.Text = Globals.FirebaseServerInteractors[indexPath.Row].ToString();

            return cell;
        }


        // Pushes the properties window
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewControllerAddDevice addDevice =
                this.owner.Storyboard.InstantiateViewController("AddManufacturer")
                    as UITableViewControllerAddDevice;
            if (addDevice != null)
            {
                this.owner.NavigationController.PushViewController(addDevice, true);
            }

        }

    }
}