using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;

namespace Hestia.DevicesScreen
{
    public class TableSourceAddDeviceDevice: UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceDevice owner;

        // List with plugins and collection they belong to
        List<string> plugins;
        string collection;

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
            return int.Parse(Resources.strings.defaultNumberOfSections);
        }

        // The number of devices in the list
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return plugins.Count;
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
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.deviceTypeCell);
            }

            // The text to display on the cell is the plugin name
            cell.TextLabel.Text = plugins[indexPath.Row];

            return cell;
        }


        // Pushes the properties window
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewControllerAddDeviceProperties addDeviceProperties = 
                owner.Storyboard.InstantiateViewController("AddDeviceProperties") 
                    as UITableViewControllerAddDeviceProperties;
            if (addDeviceProperties != null)
            {
                try
                {
                    PluginInfo requiredInfo = 
                        Globals.ServerToAddDeviceTo.GetPluginInfo(collection, plugins[indexPath.Row]);
                    addDeviceProperties.pluginInfo = requiredInfo;
                    owner.NavigationController.PushViewController(addDeviceProperties, true);
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while getting required info");
                    Console.WriteLine(ex);
                    WarningMessage message = new WarningMessage("Exception", "An exception occured on the server trying to get information for available plugins", owner);
                }
            }
        }
    }
}
