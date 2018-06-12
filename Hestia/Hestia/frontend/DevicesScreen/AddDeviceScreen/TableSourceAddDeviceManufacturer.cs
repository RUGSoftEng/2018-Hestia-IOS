using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Collections;

namespace Hestia.DevicesScreen
{
    /// <summary>
    /// This is the TableSource that defines the contents of the first screen that is displayed in 
    /// the Add devices sequence of screen. One can select the manufacturer of the device in this screen. 
    /// See, <see cref="UITableViewControllerAddDevice"/>
    /// </summary>
    public class TableSourceAddDeviceManufacturer : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDevice owner;

        // Hashtable of plugins (keyed on collection)
        Hashtable plugins;

        // The list with collections, set in the constructor. (Retrieved from server)
        List<string> collections;

        /// <summary>
        /// Constructor. Takes the device data and the ViewController
        /// </summary>
        public TableSourceAddDeviceManufacturer(List<string> collections,
              Hashtable plugins, UITableViewControllerAddDevice owner)
        {
            this.plugins = plugins;
            this.owner = owner;
            this.collections = collections;
        }

        /// <summary>
        /// We have only one section with manufacturers
        /// </summary>
        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(Resources.strings.defaultNumberOfSections);
        }

        /// <returns>The number of manufacturers in the list</returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return collections.Count;
        }

        /// <summary>
        /// Called to generate a cell to display
        /// </summary>
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

        /// <summary>
        /// Touch on row should lead to next screen with device types
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewControllerAddDeviceDevice addDeviceType =
                owner.Storyboard.InstantiateViewController(Resources.strings.viewControllerAddDevice)  as UITableViewControllerAddDeviceDevice;
            if (addDeviceType != null)
            {
                addDeviceType.collection = collections[indexPath.Row]; 
                addDeviceType.plugins = (List<string>)plugins[collections[indexPath.Row]];
                owner.NavigationController.PushViewController(addDeviceType, true);
            }
        }
    }
}
