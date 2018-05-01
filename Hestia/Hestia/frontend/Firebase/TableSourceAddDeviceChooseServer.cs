using System;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;

namespace Hestia
{
    public class TableSourceAddDeviceChooseServer : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceChooseServer owner;

        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "servercelladddevice";
        const int sections = 1;

        public TableSourceAddDeviceChooseServer(UITableViewControllerAddDeviceChooseServer owner)
        {
            this.owner = owner;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return sections;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Globals.GetNumberOfSelectedServers();
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

            cell.TextLabel.Text = Globals.GetSelectedServers().ToString();

            return cell;
        }

        // Pushes the properties window
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        { 
            Globals.serverToAddDeviceTo = Globals.GetSelectedServers()[indexPath.Row];
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