using Foundation;
using Hestia.Backend.Exceptions;
using Hestia.Backend.Models;
using Hestia.Resources;
using Hestia.Frontend.Resources;
using System;
using System.Collections.Generic;
using UIKit;

namespace Hestia.Frontend.DevicesScreen.AddDeviceScreen
{
    /// <summary>
    /// This is the TableSource that defines the contents of the second screen that is displayed in 
    /// the Add devices sequence of screen. One can select the type of device in this screen. 
    /// See, <see cref="UITableViewControllerAddDeviceDevice"/>
    /// </summary>
    public class TableSourceAddDeviceDevice: UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerAddDeviceDevice owner;

        // List with plugins and collection they belong to
        List<string> plugins;
        string collection;

        /// <summary>
        /// Constructor. Gets the device data and the ViewController
        /// </summary>
        public TableSourceAddDeviceDevice(List<string> plugins, string collection,
                    UITableViewControllerAddDeviceDevice owner)
        {
            this.plugins = plugins;
            this.owner = owner;
            this.collection = collection;
        }


        /// <summary>
        /// We have only one section with devices
        /// </summary>
        /// <returns>Number of section</returns>
        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(strings.defaultNumberOfSections);
        }

        /// <returns>Number of devices in list</returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return plugins.Count;
        }


        /// <summary>
        /// Important method. Called to generate a cell to display
        /// </summary>
        /// <returns>The cell for the indexpath in the tableView</returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(strings.deviceTypeCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, strings.deviceTypeCell);
            }

            // The text to display on the cell is the plugin name
            cell.TextLabel.Text = plugins[indexPath.Row];

            return cell;
        }

        /// <summary>
        /// If a cell is tapped, the properties window is pushed
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewControllerAddDeviceProperties addDeviceProperties = 
                owner.Storyboard.InstantiateViewController(strings.viewControllerAddDeviceProperties) 
                    as UITableViewControllerAddDeviceProperties;
            if (addDeviceProperties != null)
            {
                try
                {   // Get the required inputfield that have to be filled in for this device
                    PluginInfo requiredInfo = Globals.ServerToAddDeviceTo.GetPluginInfo(collection, plugins[indexPath.Row]);
                    addDeviceProperties.pluginInfo = requiredInfo;
                    owner.NavigationController.PushViewController(addDeviceProperties, true);
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while getting required info");
                    Console.WriteLine(ex);
                    WarningMessage.Display("Exception", "An exception occured on the server trying to get information for available plugins", owner);
                }
            }
        }
    }
}
