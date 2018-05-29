using System;
using UIKit;
using Foundation;
using System.Collections;
using Hestia.backend.models;
using Hestia.DevicesScreen.AddDeviceScreen;

namespace Hestia.DevicesScreen
{
    public class TableSourceAddDeviceProperties : UITableViewSource
    {
        // Hashtable to keep track of inputfields
        public Hashtable inputs;
        PluginInfo completeInfo;
        string[] propertyNames;

        UITableViewControllerAddDeviceProperties owner;

        // Constructor. Fill propertyNames with a copy of current keys
        public TableSourceAddDeviceProperties(
                    UITableViewControllerAddDeviceProperties owner)
        {
            this.owner = owner;
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
            return int.Parse(Resources.strings.defaultNumberOfSections);
        }

        // Important method. Called to generate a cell to display
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            PropertyCell cell = tableView.DequeueReusableCell(Resources.strings.propertyCell) as PropertyCell;
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default propertyCell
                cell = new PropertyCell((NSString)Resources.strings.propertyCell);
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
