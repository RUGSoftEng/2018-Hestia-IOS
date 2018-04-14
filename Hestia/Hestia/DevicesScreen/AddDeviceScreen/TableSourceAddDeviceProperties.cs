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
    public class TableSourceAddDeviceProperties : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceProperties owner;



        // The list with Devices, set in the constructor. (Retrieved from server)
        List<string> properties;

        Hashtable PropertyFields = new Hashtable();
            
        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "tableCellProperty";

        // Constructor. Gets the device data and the ViewController
        public TableSourceAddDeviceProperties(List<string> properties,

                    UITableViewControllerAddDeviceProperties owner)
        {
            this.properties = properties;
            this.owner = owner;
            owner.PropertyList = PropertyFields;

        }

        // We have only one section with devices (thus far)
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        // The number of manufacturers in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return properties.Count;
        }

        // Important method. Called to generate a cell to display
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }

            // The text to display on the cell is the property name
            // To be implemented: text fields
            //cell.TextLabel.Text = properties[indexPath.Row];
            UITextField propertyField = new UITextField();

            propertyField.Placeholder = properties[indexPath.Row];

            // Add the input textfield to the cell
            cell.ContentView.AddSubview(propertyField);

            // Add it to hashtable
            PropertyFields[indexPath.Row] = propertyField;

            return cell;

        }


        // Devices what happens if touch on row.
       
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            
            // to be implemented

        }

    }
}