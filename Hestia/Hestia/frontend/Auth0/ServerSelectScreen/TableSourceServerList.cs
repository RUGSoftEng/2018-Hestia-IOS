using System;
using UIKit;
using Foundation;
using Hestia.DevicesScreen.resources;
using Hestia.Auth0;

using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.frontend;

namespace Hestia.Auth0
{
    public class TableSourceServerList : UITableViewSource
    {
        ViewControllerServerList owner;

        public TableSourceServerList(ViewControllerServerList owner)
        {
            this.owner = owner;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            return int.Parse(Resources.strings.defaultNumberOfSections);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
            return Globals.Auth0Servers.Count+1;
		}

        // Sets the checkmarks
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            UITableViewCell cell = tableView.CellAt(indexPath);
            if(cell.TextLabel.Text == "Add a new device")
            {
                AddServerViewController serverView = owner.Storyboard.InstantiateViewController("AddServerViewController") as AddServerViewController;
                owner.NavigationController.PushViewController(serverView, true);
            }
            else{
                if (Globals.Auth0Servers[indexPath.Row].Selected == false)
                {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                    Globals.Auth0Servers[indexPath.Row].Selected = true;
                }
                else
                {
                    cell.Accessory = UITableViewCellAccessory.None;
                    Globals.Auth0Servers[indexPath.Row].Selected = false;
                } 
            }

            tableView.DeselectRow(indexPath, true);
		}
        		
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.serverCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.serverCell);
            }
            // The text to display on the cell is the server info
            if (indexPath.Row < Globals.Auth0Servers.Count)
            {
                cell.TextLabel.Text = Globals.Auth0Servers[indexPath.Row].Name;

                if (Globals.Auth0Servers[indexPath.Row].Selected == false)
                {
                    cell.Accessory = UITableViewCellAccessory.None;
                }
                else
                {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                }
            }
            else if(indexPath.Row >= Globals.Auth0Servers.Count)
            {
                cell.TextLabel.Text = "Add a new device";
            }

            return cell;
		}
	}
}
