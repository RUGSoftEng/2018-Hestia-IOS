using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.DevicesScreen.ActivatorScreen;
using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen.EditDevice;

namespace Hestia.DevicesScreen
{
    public class TableSource : UITableViewSource
    {
        // The viewController to which the TableView connected to this Source lives in
        UITableViewControllerDevicesMain owner;

        // Multiple Server case
        nint numberOfServers;
        List<List<Device>> serverDevices;

        Device GetSectionRow(NSIndexPath indexPath)
        {
            return serverDevices[(int)indexPath.Section][(int)indexPath.Row];
        }

        void RemoveDeviceAt(NSIndexPath indexPath)
        {
            serverDevices[(int)indexPath.Section].RemoveAt((int)indexPath.Row);
        }

        void AddDevice(NSIndexPath indexPath, Device device)
        {
            serverDevices[(int)indexPath.Section].Add(device);
        }

        // Constructor. Gets the device data (local) and the ViewController
        public TableSource(List<Device> items, UITableViewControllerDevicesMain owner)
        {
            var TableItems = items;
            this.owner = owner;
            if (Globals.LocalLogin)
            {
                numberOfServers = int.Parse(Resources.strings.defaultNumberOfServers);
                serverDevices = new List<List<Device>>();
                serverDevices.Add(TableItems);
            }
            else
            {
                serverDevices = new List<List<Device>>();
                numberOfServers = Globals.GetNumberOfSelectedServers();
                foreach (HestiaServerInteractor interactor in Globals.GetSelectedServers())
                {
                    try
                    {
                        serverDevices.Add(interactor.GetDevices());
                    } catch(ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while getting devices from server " + interactor.ToString());
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return numberOfServers;
        }

        // The number of devices in the list per section
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return serverDevices[(int)section].Count;
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

            if (serverDevices[indexPath.Section][indexPath.Row].Name != "New Device")
            {
                cell.EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                //    if (TableItems[(indexPath.Row)].Type == "Light")
                //    {
                //        cell.ImageView.Image = UIImage.FromBundle("Images/lightbulb");
                //    }
                //    if(TableItems[(indexPath.Row)].Type == "Lock")
                //    {
                //        cell.ImageView.Image = UIImage.FromBundle("Images/lock.png");
                //    }
            }

            // The text to display on the cell is the device name
            cell.TextLabel.Text = serverDevices[indexPath.Section][indexPath.Row].Name;

            return cell;
        }

        // Devices what happens if touch on row.
        // Should display the slider(s) ultimately
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (!tableView.Editing)
            {
                var d = GetSectionRow(indexPath);
                if (d.Activators.Count != 0)
                {
                    var popupNavVC = new UITableViewActivators();
                    popupNavVC.device = d;
                    nfloat heightPop = tableView.RowHeight * 2;
                    popupNavVC.PreferredContentSize = new CoreGraphics.CGSize(Globals.ScreenWidth, tableView.RowHeight * d.Activators.Count);
                    popupNavVC.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                    var popPresenter = popupNavVC.PopoverPresentationController;
                    popPresenter.SourceView = this.owner.View;
                    popPresenter.SourceRect = new CoreGraphics.CGRect(0, Globals.ScreenHeight/2-heightPop, 0, 0);
                    popPresenter.Delegate = new PopoverDelegate();
                    popPresenter.PermittedArrowDirections = 0;
                    popPresenter.BackgroundColor = UIColor.White;
                    this.owner.PresentViewController(popupNavVC, true, null);
                }
                tableView.DeselectRow(indexPath, true);
            }
            // Go to edit name window for non-insert cells
            else if (tableView.Editing && tableView.CellAt(indexPath).EditingStyle != UITableViewCellEditingStyle.Insert)
            {
                UIViewControllerEditDeviceName editViewController = new UIViewControllerEditDeviceName(this.owner);
                editViewController.device = GetSectionRow(indexPath);
                this.owner.NavigationController.PushViewController(editViewController, true);
                tableView.DeselectRow(indexPath, true);
            }
            else if (tableView.Editing && tableView.CellAt(indexPath).EditingStyle == UITableViewCellEditingStyle.Insert)
            {
                InsertAction();
                tableView.DeselectRow(indexPath, true);
            }
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            switch (editingStyle)
            {
                case UITableViewCellEditingStyle.Delete:
                    if (Globals.LocalLogin)
                    {
                        try
                        {
                            // remove device from server 
                            Globals.LocalServerinteractor.RemoveDevice(serverDevices[indexPath.Section][indexPath.Row]);
                        }
                        catch (ServerInteractionException ex)
                        {
                            Console.WriteLine("Exception while removing device");
                            Console.Out.WriteLine(ex.ToString());
                        }
                    }
                    else
                    {
                        var deviceInRow = serverDevices[indexPath.Section][indexPath.Row];
                        var deviceNetworkHanlder = deviceInRow.NetworkHandler;
                        var deviceServerInteractor = new HestiaServerInteractor(deviceNetworkHanlder);
                        try
                        {
                            deviceServerInteractor.RemoveDevice(deviceInRow);
                        }
                        catch (ServerInteractionException ex)
                        {
                            Console.WriteLine("Exception while removing device");
                            Console.Out.WriteLine(ex.ToString());
                        }
                    }
                    RemoveDeviceAt(indexPath);

                    // delete the row from the table
                    tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                    break;
                case UITableViewCellEditingStyle.None:
                    Console.WriteLine("CommitEditingStyle:None called");
                    break;
                case UITableViewCellEditingStyle.Insert:
                    InsertAction();
                    break;
            }
        }

        public void InsertAction()
        {
            if (Globals.LocalLogin)
            {
                Globals.ServerToAddDeviceTo = Globals.LocalServerinteractor;
                UITableViewControllerAddDevice addDeviceVc =
                    this.owner.Storyboard.InstantiateViewController("AddManufacturer")
                         as UITableViewControllerAddDevice;
                owner.NavigationController.PushViewController(addDeviceVc, true);
            }
            else
            {
                var addDeviceChooseServer =
                    this.owner.Storyboard.InstantiateViewController("AddDeviceChooseServer") as UITableViewControllerAddDeviceChooseServer;
                owner.NavigationController.PushViewController(addDeviceChooseServer, true);
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                return true; // return false if you wish to disable editing for a specific indexPath or for all rows
            }
            else
            {
                return false;
            }
        }

        // Defines the red delete/add buttons before cell
        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Editing)
            {
                // Add new device below table
                if (indexPath.Row == tableView.NumberOfRowsInSection(tableView.NumberOfSections() - 1) - 1
                    && indexPath.Section == tableView.NumberOfSections() - 1)
                    return UITableViewCellEditingStyle.Insert;
                else
                    return UITableViewCellEditingStyle.Delete;
            }
            else // not in editing mode, enable swipe-to-delete for all rows
                return UITableViewCellEditingStyle.Delete;
            // This above should change to pop-up for delete confirmation
        }

        // Is called after a press on edit button
        public void WillBeginTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();
            // insert the 'ADD NEW' row at the end of table display
            var insertIndexpath = new NSIndexPath[] {
                NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (tableView.NumberOfSections() - 1),
                                            tableView.NumberOfSections() - 1)
            };

            tableView.InsertRows(insertIndexpath, UITableViewRowAnimation.Fade);

            // create a new item and add it to our underlying data 
            // This should not be permanently stored, but trigger the add new
            // device screen on touch
            List<backend.models.Activator> temp_activator = new List<backend.models.Activator>();
            AddDevice(insertIndexpath[0], new Device(" ", "New Device", " ", temp_activator, Globals.GetTemporyNetworkHandler()));

            tableView.EndUpdates(); // applies the changes
        }

        public void DidFinishTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();
         
            var insertIndexpath = new NSIndexPath[] {
                NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (tableView.NumberOfSections() - 1) - 1,
                                            tableView.NumberOfSections() - 1)};

            RemoveDeviceAt(insertIndexpath[0]);
            tableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(tableView.NumberOfRowsInSection(tableView.NumberOfSections() - 1) - 1, 
                                            tableView.NumberOfSections() - 1) }, UITableViewRowAnimation.Fade);
            
            tableView.EndUpdates(); // applies the changes
        }

		public override string TitleForHeader(UITableView tableView, nint section)
		{
            if(Globals.LocalLogin)
            {
                return Globals.LocalServerinteractor.ToString();
            }
            else
            {
                return Globals.GetSelectedServers()[(int)section].ToString();
            }
        }
   	}
}
