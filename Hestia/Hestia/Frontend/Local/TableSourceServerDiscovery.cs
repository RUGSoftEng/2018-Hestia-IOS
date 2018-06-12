using Foundation;
using Hestia.Resources;
using System;
using UIKit;

namespace Hestia.Frontend.Local
{
    public class TableSourceServerDiscovery: UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
		UITableViewControllerServerDiscovery owner;

        // List with plugins and collection they belong to
		NSMutableArray services = new NSMutableArray();

        public void UpdateServices(NSMutableArray newservices)
        {
            this.services = newservices;
        }
        // Constructor. Gets the device data and the ViewController
		public TableSourceServerDiscovery(NSMutableArray services,
		                UITableViewControllerServerDiscovery owner)
        {
			this.services = services;
            this.owner = owner;
        }
        
        // We have only one section with devices (thus far)
        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(strings.defaultNumberOfSections);
        }

        // The number of devices in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return (nint)services.Count;
        }

        // Important method. Called to generate a cell to display
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(strings.deviceTypeCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, "serverdiscoverycell");
            }

            // The text to display on the cell is the plugin name
            cell.TextLabel.Text = services.GetItem<NSNetService>((nuint)indexPath.Row).HostName;
	
            
            return cell;
        }


        // Should fill in the fields in the previous screen
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
           // Get serverInteractor for that row, performsegue
        }  
    }
}
