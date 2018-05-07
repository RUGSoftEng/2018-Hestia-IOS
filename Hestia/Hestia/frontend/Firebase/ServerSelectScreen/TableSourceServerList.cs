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

        public TableSourceServerList(List<WebServer> serverlist, UITableViewServerList owner)
        {
            Globals.Auth0Servers = serverlist;
            this.owner = owner;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            return int.Parse(Resources.strings.defaultNumberOfSections);
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
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.serverCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.serverCell);
            }
            // The text to display on the cell is the server info
            cell.TextLabel.Text = Globals.Auth0Servers[indexPath.Row].Interactor.ToString();

            return cell;
		}
	}
}
