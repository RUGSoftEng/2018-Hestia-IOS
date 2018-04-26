using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia
{
    public class TableSourceServerList : UITableViewSource
    {
        UITableViewServerList owner;
        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "servercell";

        public TableSourceServerList(List<FireBaseServer> item, UITableViewServerList owner)
        {
            Globals.FirebaseServers = item;
            this.owner = owner;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
            return Globals.FirebaseServers.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            UITableViewCell cell = tableView.CellAt(indexPath);

            if (Globals.FirebaseServers[indexPath.Row].Selected == false)
            {
                cell.Accessory = UITableViewCellAccessory.Checkmark;
                Globals.FirebaseServers[indexPath.Row].Selected = true;
            }
            else
            {
                cell.Accessory = UITableViewCellAccessory.None;
                Globals.FirebaseServers[indexPath.Row].Selected = false;
            }
            tableView.DeselectRow(indexPath, true);
		}


		
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
            cell.TextLabel.Text = Globals.FirebaseServers[indexPath.Row].Interactor.getIP();

            return cell;
		}
	}
}
