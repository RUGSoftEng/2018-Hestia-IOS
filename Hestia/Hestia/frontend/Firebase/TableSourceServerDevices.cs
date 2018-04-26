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
    public class TableSourceServerDevices : UITableViewSource
    {
        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "serverdevicecell";

        UITableViewControllerServerDevice owner;
        List<Device> devices = new List<Device>();

        public TableSourceServerDevices(UITableViewControllerServerDevice owner)
        {
            foreach (FireBaseServer firebaseserver in Globals.FirebaseServers)
            {
                if (firebaseserver.Selected == true)
                {
                    devices.AddRange(firebaseserver.Interactor.GetDevices());
                }
            }
            this.owner = owner;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }

            cell.TextLabel.Text = devices[indexPath.Row].Name;


            return cell;
		}

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            
            return devices.Count;
        }
	}
}
