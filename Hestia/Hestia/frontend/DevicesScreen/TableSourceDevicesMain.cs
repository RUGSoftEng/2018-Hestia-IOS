using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using Hestia.DevicesScreen.ActivatorScreen;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen.EditDevice;
using Hestia.frontend;
using Hestia.Resources;

namespace Hestia.DevicesScreen
{
    public class TableSourceDevicesMain : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerDevicesMain owner;

        // Multiple Server case
        public nint numberOfServers;
        public List<List<Device>> serverDevices;

        Device GetSectionRow(NSIndexPath indexPath)
        {
            return serverDevices[indexPath.Section][indexPath.Row];
        }

        void RemoveDeviceAt(NSIndexPath indexPath)
        {
            serverDevices[indexPath.Section].RemoveAt(indexPath.Row);
        }

        // Constructor. Gets the device data (local) and the ViewController
        public TableSourceDevicesMain(UITableViewControllerDevicesMain owner)
        {
            this.owner = owner;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return numberOfServers;
        }

        // The number of devices in the list per section
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (serverDevices.Count <= 0)
            {
                return 0;
            }
            int count = serverDevices[(int)section].Count;
            return count;
        }

        // Important method. Called to generate a cell to display
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

        // Displays activators or go to edit device screen when in editing mode
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (!tableView.Editing)
            {
                var deviceRow = GetSectionRow(indexPath);
                if (deviceRow.Activators.Count != 0)
                {
                    var popupViewController = new UITableViewActivators();
                    popupViewController.device = deviceRow;
                    nfloat heightPop = tableView.RowHeight * 2;
                    popupViewController.PreferredContentSize = new CoreGraphics.CGSize(Globals.ScreenWidth, tableView.RowHeight * deviceRow.Activators.Count);
                    popupViewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                    var popPresenter = popupViewController.PopoverPresentationController;
                    popPresenter.SourceView = owner.View;
                    popPresenter.SourceRect = new CoreGraphics.CGRect(0, Globals.ScreenHeight / 2 - heightPop, 0, 0);
                    popPresenter.Delegate = new PopoverDelegate();
                    popPresenter.PermittedArrowDirections = 0;
                    popPresenter.BackgroundColor = UIColor.White;
                    owner.PresentViewController(popupViewController, true, null);
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
                    Console.WriteLine("Exception while removing device");
                    Console.Out.WriteLine(ex);
                    WarningMessage message = new WarningMessage("Exception while removing device", "An exception occurred while removing the device from the local server", owner);
                }
            }
            else
            {
                var deviceInRow = serverDevices[indexPath.Section][indexPath.Row];
				var deviceServerInteractor = deviceInRow.ServerInteractor;
                try
                {
                    deviceServerInteractor.RemoveDevice(deviceInRow);
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while removing device");
                    Console.Out.WriteLine(ex);
                    WarningMessage message = new WarningMessage("Exception while removing device", "An exception occurred while removing the device from the local server through the Auth0 server", owner);
                }
            }
            // Remove device from local list
            RemoveDeviceAt(indexPath);

            // delete the row from the table
            tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
        }

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

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                return true; // return false if you wish to disable editing for a specific indexPath or for all rows
            }
            return false;
        }

        // Is called after a press on edit button
        public void WillBeginTableEditing(UITableView tableView)
        {
            tableView.TableHeaderView = owner.GetTableViewHeader(true);
        }

        public void DidFinishTableEditing(UITableView tableView)
        {
            tableView.TableHeaderView = owner.GetTableViewHeader(false);
        }

		public override string TitleForHeader(UITableView tableView, nint section)
		{
            if(Globals.LocalLogin)
            {
                return Globals.ServerName + " " + Globals.IP + ":" + Globals.Port;
            }

            HestiaServer server = Globals.GetSelectedServers()[(int)section];
            return server.Name + " " + server.Ip + ":" + server.Port;
        }
   	}
}
