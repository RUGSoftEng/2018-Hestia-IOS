using Foundation;
using Hestia.Backend.Models;
using Hestia.Resources;
using System;
using System.Collections;
using UIKit;

namespace Hestia.Frontend.DevicesScreen.AddDeviceScreen
{
    /// <summary>
    /// This is the TableSource that defines the contents of the third and final screen that is displayed in 
    /// the Add devices sequence of screen. One can enter the properties of the device in this screen. 
    /// This TableSource has dynamic contents, that means that the required info
    /// for each device will be different, it depends on the type of device.
    /// See, <see cref="UITableViewControllerAddDeviceProperties"/>
    /// </summary>
    public class TableSourceAddDeviceProperties : UITableViewSource
    {
        // Hashtable to keep track of inputfields
        public Hashtable inputs;
        PluginInfo completeInfo;
        string[] propertyNames;

        UITableViewControllerAddDeviceProperties owner;

        /// <summary>
        /// Constructor. Fills propertyNames with a copy of current keys to display in the inputfields as placeholder.
        /// See, <see cref="PropertyCell"/>.
        /// </summary>
        public TableSourceAddDeviceProperties(UITableViewControllerAddDeviceProperties owner)
        {
            inputs = owner.inputFields;
            completeInfo = owner.pluginInfo;
            propertyNames = new string[completeInfo.RequiredInfo.Keys.Count];
            completeInfo.RequiredInfo.Keys.CopyTo(propertyNames, 0);
        }

        // We have only one section 
        public override nint NumberOfSections(UITableView tableView)
        {
            return completeInfo.RequiredInfo.Keys.Count;
        }

        // The number of properties in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return int.Parse(strings.defaultNumberOfSections);
        }

        // Important method. Called to generate a cell to display
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            PropertyCell cell = tableView.DequeueReusableCell(strings.propertyCell) as PropertyCell;
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default propertyCell
                cell = new PropertyCell((NSString)strings.propertyCell);
            }

            // Keep reference in hashtable
            inputs[propertyNames[indexPath.Section]] = cell;

            return cell;
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return completeInfo.RequiredInfo[propertyNames[section]];
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return propertyNames[section];
        }
	}
}
