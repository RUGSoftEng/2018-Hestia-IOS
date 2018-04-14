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
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceProperties owner;



        // The list with Devices, set in the constructor. (Retrieved from server)
        //List<string> properties;
        public Hashtable inputs;
        PluginInfo completeInfo;
        string[] propertyNames;
            
        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "tableCellProperty";

        // Constructor. Gets the device data and the ViewController
        public TableSourceAddDeviceProperties(
                    UITableViewControllerAddDeviceProperties owner)
        {
            inputs = owner.inputFields;
            this.completeInfo = owner.pluginInfo;
            propertyNames = new string[completeInfo.RequiredInfo.Keys.Count];
            completeInfo.RequiredInfo.Keys.CopyTo(propertyNames, 0);
            this.owner = owner;

        }

        // We have only one section with devices (thus far)
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        // The number of manufacturers in the list
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
                // Generate a default table cell
                cell = new PropertyCell((NSString)CellIdentifier);
            }

           
            cell.UpdateCell(propertyNames[indexPath.Row]);
            inputs[propertyNames[indexPath.Row]] = cell;
            // The text to display on the cell is the property name
            // To be implemented: text fields
            //cell.TextLabel.Text = propertyNames[indexPath.Row];

           // 
           
          //  propertyField.Placeholder = propertyNames[indexPath.Row];
          //  cell.ContentView.BackgroundColor = UIColor.Cyan;
          //  UILabel test = new UILabel();
           // test.Text = " test";
            // Add the input textfield to the cell
           // cell.ContentView.Add(test);

            // Add it to dictionary
          //  completeInfo.RequiredInfo[propertyNames[indexPath.Row]] = propertyField.Text;

            return cell;

        }

       

        // Devices what happens if touch on row.
       
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            
            // to be implemented

        }

    }
}