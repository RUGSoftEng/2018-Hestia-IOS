using System;
using UIKit;
using Foundation;
using Hestia.DevicesScreen.resources;
using Hestia.backend.models;
using Hestia.backend.exceptions;
using Hestia.frontend;

namespace Hestia
{
    /// <summary>
    /// This class defines the contents and behaviour of the TableView living in <see cref="ViewControllerChooseServer"/>.
    /// It the list of locals servers that were selected to be shown in the devices main screen.
    /// One can select the server that the new device should be added to.
    /// </summary>
    public class TableSourceAddDeviceChooseServer : UITableViewSource
    {
        // The viewController in which the TableView connected to this Source lives in
        ViewControllerChooseServer owner;

        public TableSourceAddDeviceChooseServer(ViewControllerChooseServer owner)
        {
            this.owner = owner;
        }

        /// <returns>There is only one section: the list with servers</returns>
        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(Resources.strings.defaultNumberOfSections);
        }

        /// <returns>The number rows is the number of selected servers</returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Globals.GetNumberOfSelectedServers()+ 1;
        }

        /// <summary>
        /// Important method. Called to generate a cell to display. It displays the name of the server.
        /// </summary>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.addDeviceChooseServerCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.addDeviceChooseServerCell);
            }
            // The text to display on the cell is the server info
            if (indexPath.Row < Globals.Auth0Servers.Count)
            {
                cell.TextLabel.Text = Globals.GetSelectedServers()[indexPath.Row].Name;
            }
            else if (indexPath.Row >= Globals.Auth0Servers.Count)
            {
                cell.TextLabel.Text = "Add a new server";
            }


            return cell;
        }

        // Pushes the Important method. Called to generate a cell to display
        /// <summary>
        /// When a row is selected, the <see cref="UITableViewControllerAddDevice"/> is pushed, 
        /// which further manages the adding of a device.
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        { 
            UITableViewCell cell = tableView.CellAt(indexPath);

            if (cell.TextLabel.Text == "Add a new server")
            {
                AddServerViewController serverView = owner.Storyboard.InstantiateViewController("AddServer") as AddServerViewController;
                owner.NavigationController.PushViewController(serverView, true);
            }
            else{
                Globals.ServerToAddDeviceTo = Globals.GetInteractorsOfSelectedServers()[indexPath.Row];
                UITableViewControllerAddDevice addDevice =
                owner.Storyboard.InstantiateViewController(Resources.strings.AddManufacturerViewController)
                    as UITableViewControllerAddDevice;
                if (addDevice != null)
                {
                    owner.NavigationController.PushViewController(addDevice, true);
                }
            }
            tableView.DeselectRow(indexPath,true);
        }
    }
}


