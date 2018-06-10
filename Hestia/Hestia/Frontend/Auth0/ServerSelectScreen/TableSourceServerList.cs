using System;
using UIKit;
using Foundation;
using Hestia.Resources;
using Hestia.Frontend.Resources;

namespace Hestia.Frontend.Auth0.ServerSelectScreen
{
    public class TableSourceServerList : UITableViewSource
    {
		public override nint NumberOfSections(UITableView tableView)
		{
            return int.Parse(strings.defaultNumberOfSections);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
            return Globals.Auth0Servers.Count;
		}

        // Sets the checkmarks
		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            UITableViewCell cell = tableView.CellAt(indexPath);

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
            tableView.DeselectRow(indexPath, true);
		}
        		
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(strings.serverCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, strings.serverCell);
            }
            // The text to display on the cell is the server info
            cell.TextLabel.Text = Globals.Auth0Servers[indexPath.Row].Name;
            if (Globals.Auth0Servers[indexPath.Row].Selected == false)
            {
                cell.Accessory = UITableViewCellAccessory.None;
            }
            else
            {
                cell.Accessory = UITableViewCellAccessory.Checkmark;
            }

            return cell;
		}
	}
}
