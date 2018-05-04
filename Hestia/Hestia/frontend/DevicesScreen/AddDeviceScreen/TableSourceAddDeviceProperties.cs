using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
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
        string info;

        // Constructor. Fill propertyNames with a copy of current keys
        public TableSourceAddDeviceProperties(
                    UITableViewControllerAddDeviceProperties owner)
        {
            this.owner = owner;
            inputs = owner.inputFields;
            this.completeInfo = owner.pluginInfo;
            propertyNames = new string[completeInfo.RequiredInfo.Keys.Count];
            completeInfo.RequiredInfo.Keys.CopyTo(propertyNames, 0);
        }

        // We have only one section 
        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(Resources.strings.defaultNumberOfSections);
        }

        // The number of properties in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return completeInfo.RequiredInfo.Keys.Count;
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

            // Put the property names in the placeholder filed
            cell.UpdateCell(propertyNames[indexPath.Row]);
            cell.Accessory = UITableViewCellAccessory.DetailButton;
            // Keep reference in hashtable
            inputs[propertyNames[indexPath.Row]] = cell;

            return cell;

        }

		public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
		{
            if(propertyNames[indexPath.Row]=="ip")
            {
                info = "The IP should be like X.X.X.X, where X has to be between 0 and 255";
            }
            if(propertyNames[indexPath.Row]=="name")
            {
                info = "The name can be whatever you want";
            }
            if(propertyNames[indexPath.Row] == "port")
            {
                info = "Insert the correspondat port of the server";
            }
            
            UIAlertController alertController = UIAlertController.Create(propertyNames[indexPath.Row], info , UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            
            owner.PresentViewController(alertController, true, null);
            tableView.DeselectRow(indexPath, true);
		}

	}
}