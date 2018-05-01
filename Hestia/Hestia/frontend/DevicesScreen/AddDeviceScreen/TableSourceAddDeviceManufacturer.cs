using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen
{
    public class TableSourceAddDeviceManufacturer : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDevice owner;

        // Hashtable of plugins (keyed on collection)
        Hashtable plugins;

        // The list with collections, set in the constructor. (Retrieved from server)
        List<string> collections;

        // Constructor. Gets the device data and the ViewController
        public TableSourceAddDeviceManufacturer(List<string> collections,
                                    Hashtable plugins,
                    UITableViewControllerAddDevice owner)
        {
            this.plugins = plugins;
            this.owner = owner;
            this.collections = collections;
        }

        // We have only one section with manufacturers
        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(Resources.strings.defaultNumberOfSections);
        }

        // The number of manufacturers in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return collections.Count;
        }

        // Important method. Called to generate a cell to display
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.manufacturerCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.manufacturerCell);
            }

            // The text to display on the cell is the manufacturer name
            cell.TextLabel.Text = collections[indexPath.Row];
           
            return cell;
        }


        // Touch on row should lead to next screen with device types
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewControllerAddDeviceDevice addDeviceType =
                this.owner.Storyboard.InstantiateViewController("AddDevice") 
                    as UITableViewControllerAddDeviceDevice;
            if (addDeviceType != null)
            {
                addDeviceType.collection = collections[indexPath.Row]; 
                addDeviceType.plugins = (List<string>)this.plugins[collections[indexPath.Row]];
                this.owner.NavigationController.PushViewController(addDeviceType, true);
            }

        }

    }
}