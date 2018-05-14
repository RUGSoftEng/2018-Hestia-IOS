using System;
using System.Collections.Generic;
using UIKit;
using Foundation;


using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen.resources;

namespace Hestia.DevicesScreen
{
    public class TableSourceServerDiscovery: UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
		UITableViewControllerServerDiscovery owner;

        // List with plugins and collection they belong to
		NSMutableArray services = new NSMutableArray();
        
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
            return int.Parse(Resources.strings.defaultNumberOfSections);
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
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.deviceTypeCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, "cellIdentifier");
            }

            // The text to display on the cell is the plugin name
			//cell.TextLabel.Text = services[indexPath.Row];
            
            return cell;
        }


        // Pushes the properties window
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
           // Get serverInteractor for that row, performsegue
        }  
    }
}
