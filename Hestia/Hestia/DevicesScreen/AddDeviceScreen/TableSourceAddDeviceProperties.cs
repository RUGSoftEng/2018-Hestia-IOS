﻿using System;
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
            
        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "tableCellProperty";

        // Constructor. Fill propertyNames with a copy of current keys
        public TableSourceAddDeviceProperties(
                    UITableViewControllerAddDeviceProperties owner)
        {
            inputs = owner.inputFields;
            this.completeInfo = owner.pluginInfo;
            propertyNames = new string[completeInfo.RequiredInfo.Keys.Count];
            completeInfo.RequiredInfo.Keys.CopyTo(propertyNames, 0);
        }

        // We have only one section 
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
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
            PropertyCell cell = tableView.DequeueReusableCell(CellIdentifier) as PropertyCell;
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default propertyCell
                cell = new PropertyCell((NSString)CellIdentifier);
            }

            // Put the property names in the placeholder filed
            cell.UpdateCell(propertyNames[indexPath.Row]);
            // Keep reference in hashtable
            inputs[propertyNames[indexPath.Row]] = cell;

            return cell;

        }

    }
}