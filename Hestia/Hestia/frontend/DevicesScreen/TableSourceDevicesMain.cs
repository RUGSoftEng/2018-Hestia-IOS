using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using Hestia.DevicesScreen.ActivatorScreen;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen.EditDevice;
using Hestia.Resources;
using System.Diagnostics.Contracts;

namespace Hestia.DevicesScreen
{   /// <summary>
    /// This class contains the behaviour of the TableView that shows the list of devices.
    /// <see cref="UITableViewControllerDevicesMain"/>
    /// </summary>
    public class TableSourceDevicesMain : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerDevicesMain owner;

        // Multiple Server case
        public nint numberOfServers;
        public List<List<Device>> serverDevices;

        /// <returns>The device</returns>
        /// <param name="indexPath">The indexpath of the cell on which the device should be shown</param>
        Device GetSectionRow(NSIndexPath indexPath)
        {
            Contract.Ensures(Contract.Result<Device>() != null);
            return serverDevices[indexPath.Section][indexPath.Row];
        }

        void RemoveDeviceAt(NSIndexPath indexPath)
        {
            serverDevices[indexPath.Section].RemoveAt(indexPath.Row);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Hestia.DevicesScreen.TableSourceDevicesMain"/> class.
        /// </summary>
        /// <param name="owner">The ViewController in which this TableView lives</param>
        public TableSourceDevicesMain(UITableViewControllerDevicesMain owner)
        {
            this.owner = owner;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return numberOfServers;
        }

        /// <summary>
        /// The number of devices in the list per section
        /// </summary>
        /// <returns>The number of devices in a section</returns>
        /// <param name="section">The section number</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (serverDevices.Count <= 0)
            {
                return 0;
            }
            int count = serverDevices[(int)section].Count;
            return count;
        }

        /// <summary>
        /// Important method. Called to generate a cell to display.
        /// It puts the name on the cell and shows a disclosure indicator in editing mode.
        /// </summary>
        /// <returns>The cell with the device name.</returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.devicesMainCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.devicesMainCell);
            }

            cell.EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
            cell.Accessory = UITableViewCellAccessory.None;

            // The text to display on the cell is the device name
            cell.TextLabel.Text = serverDevices[indexPath.Section][indexPath.Row].Name;

            return cell;
        }

        /// <summary>
        /// Defines the action when tapped on a cell. It displays activators in a pop up
        /// or goes to the edit device screen when in editing mode.
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (!tableView.Editing)
            {
                var device = GetSectionRow(indexPath);
                if (device.Activators.Count != 0)
                {
                    // Create the contents of the pop up window, which is a list of activators. 
                    var popupNavVC = new UITableViewActivators();
                    popupNavVC.Title = device.Name;
                    popupNavVC.device = device;

                    var navigationController = new UINavigationController(popupNavVC);
                    navigationController.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                    navigationController.PreferredContentSize = new CoreGraphics.CGSize(Globals.ScreenWidth, tableView.RowHeight * device.Activators.Count);

                    // Define the layout of the pop up
                    nfloat heightPop = 20 + navigationController.NavigationBar.Frame.Size.Height;
                    var popPresenter = navigationController.PopoverPresentationController;
                    popPresenter.SourceView = owner.View;
                    popPresenter.SourceRect = new CoreGraphics.CGRect(0, Globals.ScreenHeight/2-heightPop, 0, 0);
                    popPresenter.Delegate = new PopoverDelegate();
                    popPresenter.PermittedArrowDirections = 0;
                    popPresenter.BackgroundColor = UIColor.White;
                    // Show the pop up
                    owner.PresentViewController(navigationController, true, null);
                }
            }
            // Go to edit name window for non-insert cells
            else
            {
                UIViewControllerEditDeviceName editViewController = new UIViewControllerEditDeviceName(this.owner);
                editViewController.device = GetSectionRow(indexPath);
                owner.NavigationController.PushViewController(editViewController, true);
            }
            tableView.DeselectRow(indexPath, true);
        }

        /// <summary>
        /// Defines what happens when the delete action is confirmed. 
        /// Then the device is removed from the server and the list.
        /// </summary>
        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {   
            if (Globals.LocalLogin)
            {
                try
                {   // remove device from server   
                    Globals.LocalServerinteractor.RemoveDevice(serverDevices[indexPath.Section][indexPath.Row]);
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while removing device. (Bug in server: exception is always thrown)");
                    Console.Out.WriteLine(ex);
                }
            }
            else // Global login
            {
                var deviceInRow = serverDevices[indexPath.Section][indexPath.Row];
				var deviceServerInteractor = deviceInRow.ServerInteractor;
                try
                {
                    deviceServerInteractor.RemoveDevice(deviceInRow);
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while removing device. (Bug in server: exception is always thrown)");
                    Console.Out.WriteLine(ex);
                }
            }
            // Remove device from list that is shown in the TableView
            RemoveDeviceAt(indexPath);

            // Delete the row from the table
            tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
        }

        /// <summary>
        /// The action that is performed when the green plus icon is tapped to add a new device.
        /// It segues to the add device screen in case of local login or to the server select screen
        /// in case of global login, to choose the server to add the device to.
        /// <see cref="ViewControllerChooseServer"/> 
        /// <see cref="UITableViewControllerAddDevice"/>
        /// </summary>
        public void InsertAction()
        {
            if (Globals.LocalLogin)
            {
                Globals.ServerToAddDeviceTo = Globals.LocalServerinteractor;
                var addDeviceViewController =
                    owner.Storyboard.InstantiateViewController(strings.AddManufacturerViewController);
                if (addDeviceViewController != null)
                {
                    owner.NavigationController.PushViewController(addDeviceViewController, true);
                }
            }
            else
            {
                var addDeviceChooseServer =
                    owner.Storyboard.InstantiateViewController(strings.AddDeviceChooseServerViewController);
                if (addDeviceChooseServer != null)
                {
                    owner.NavigationController.PushViewController(addDeviceChooseServer, true);
                }
            }
        }

        /// <summary>
        /// The rows cannot be edited in normal mode, but they can in editing mode.
        /// That is: they can be deleted.
        /// </summary>
        /// <returns><c>true</c>, if row can be edited <c>false</c> otherwise.</returns>
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Is called after a press on edit button. It refreshes the header such that
        /// the microphone icon is changed to an insertion icon.
        /// </summary>
        public void WillBeginTableEditing(UITableView tableView)
        {
            tableView.TableHeaderView = owner.GetTableViewHeader(true);
        }

        /// <summary>
        /// Is called after a press on done button. It refreshes the header such that
        /// the insertion icon is changed back to microphone icon.
        /// </summary>
        public void DidFinishTableEditing(UITableView tableView)
        {
            tableView.TableHeaderView = owner.GetTableViewHeader(false);
        }

        /// <returns>The title for the header, which consist of the server name + its IP and port</returns>
		public override string TitleForHeader(UITableView tableView, nint section)
		{
            if(Globals.LocalLogin)
            {
                return Globals.ServerName + " " + Globals.Address + ":" + int.Parse(strings.defaultPort);
            }

            HestiaServer server = Globals.GetSelectedServers()[(int)section];
            return server.Name + " " + server.Address + ":" + server.Port;
        }
   	}
}
