using System;
using UIKit;
using Foundation;
using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.DevicesScreen.EditDevice;
using Hestia.Resources;
using Hestia.frontend.Auth0.ServerSelectScreen;

namespace Hestia.Auth0
{
    public class TableSourceServerList : UITableViewSource
    {
        ViewControllerServerList owner;

        public TableSourceServerList(ViewControllerServerList owner)
        {
            this.owner = owner;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return int.Parse(strings.defaultNumberOfSections);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Globals.Auth0Servers.Count;
        }

        // Sets the checkmarks
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.CellAt(indexPath);
            if (!owner.Editing)
            {
                if (Globals.Auth0Servers[indexPath.Row].Selected == false)
                {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                    Globals.Auth0Servers[indexPath.Row].Selected = true;
                }
                else
                {
                    cell.Accessory = UITableViewCellAccessory.None;
                    Globals.Auth0Servers[indexPath.Row].Selected = false;
                }

            }
            // Go to edit name window for non-insert cells
            else
            {
                UIViewControllerEditServerName uIViewControllerEditServerName = new UIViewControllerEditServerName(this.owner);
                uIViewControllerEditServerName.server = Globals.Auth0Servers[indexPath.Row];
               // editViewController.device = GetSectionRow(indexPath);
                owner.NavigationController.PushViewController(uIViewControllerEditServerName, true);
            }

            tableView.DeselectRow(indexPath, true);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(strings.serverCell);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default table cell
                cell = new UITableViewCell(UITableViewCellStyle.Default, strings.serverCell);
            }

            cell.EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;


                cell.TextLabel.Text = Globals.Auth0Servers[indexPath.Row].Name;

                if (Globals.Auth0Servers[indexPath.Row].Selected == false)
                {
                    cell.Accessory = UITableViewCellAccessory.None;
                }
                else
                {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                }


            return cell;
        }

        /// <summary>
        /// Defines what happens when the delete action is confirmed. 
        /// Then the device is removed from the server and the list.
        /// </summary>
        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {

            //var deviceInRow = serverDevices[indexPath.Section][indexPath.Row];
           // var deviceServerInteractor = deviceInRow.ServerInteractor;
            try
            {
                //deviceServerInteractor.RemoveDevice(deviceInRow);
                Globals.HestiaWebServerInteractor.DeleteServer(Globals.Auth0Servers[indexPath.Row]);
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while removing device. (Bug in server: exception is always thrown)");
                Console.Out.WriteLine(ex);
            }

            //Remove device from list that is shown in the TableView
            //RemoveDeviceAt(indexPath);

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
            AddServerViewController serverView = owner.Storyboard.InstantiateViewController("AddServer") as AddServerViewController;
            owner.NavigationController.PushViewController(serverView, true);
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
        /// Is called after a press on edit button. The microphone icon is changed to an insertion icon.
        /// </summary>
        public void WillBeginTableEditing(UITableView tableView)
        {
            owner.ReloadButtons(true);
        }

        /// <summary>
        /// Is called after a press on done button. The insertion icon is changed back to microphone icon.
        /// </summary>
        public void DidFinishTableEditing(UITableView tableView)
        {
            owner.ReloadButtons(false);
        }
    }
}

