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
    public class TableSourceAddDeviceDevice: UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceDevice owner;

        // List with plugins and collection they belong to
        List<string> plugins;
        string collection;

        // The kind of cell that is used in the table (set in Storyboard)
        string CellIdentifier = "tableCellDeviceType";

        // Constructor. Gets the device data and the ViewController
        public TableSourceAddDeviceDevice(List<string> plugins,
                                          string collection,
                    UITableViewControllerAddDeviceDevice owner)
        {
            this.plugins = plugins;
            this.owner = owner;
            this.collection = collection;

        }

        // We have only one section with devices (thus far)
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        // The number of manufacturers in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return plugins.Count;
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

            // The text to display on the cell is the plugin name
            cell.TextLabel.Text = plugins[indexPath.Row];

            return cell;

        }


        // Pushes the properties window
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
          
            UITableViewControllerAddDeviceProperties addDeviceProperties = 
                this.owner.Storyboard.InstantiateViewController("AddDeviceProperties") 
                    as UITableViewControllerAddDeviceProperties;
            if (addDeviceProperties != null)
            {

                try
                {
                 PluginInfo requiredInfo = 
                        Globals.ServerInteractor.GetPluginInfo(collection, (string)this.plugins[indexPath.Row]);
                    //var keys = requiredInfo.RequiredInfo.Keys;

                    //foreach (var key in keys)
                    // {
                    //    Console.WriteLine("key");
                    //    Console.WriteLine(key);
                    //    addDeviceProperties.properties.Add(key);
                    //}
                    addDeviceProperties.pluginInfo = requiredInfo;
                    this.owner.NavigationController.PushViewController(addDeviceProperties, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception while getting required info");
                    Console.WriteLine(e.StackTrace);
                }
            }

        }

    }
}