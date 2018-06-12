using System;
using UIKit;
using Foundation;
using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.Resources;
using Hestia.frontend.Auth0.ServerSelectScreen;
using Hestia.frontend;

namespace Hestia.Auth0
{
    /// <summary>
    /// This class define how the server list should be displayed.
    /// Furthermore, this method add a new row showing that we can
    /// add a new server.
    /// </summary>
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

        /// <summary>
        /// This method allows the user to select and deselect the server that he wants to show
        /// in the main screen.
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.CellAt(indexPath);
            if (!owner.TableView.Editing)
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
            else  // Go to edit name window
            {
                UIViewControllerEditServerName uIViewControllerEditServerName = new UIViewControllerEditServerName(this.owner);
                uIViewControllerEditServerName.server = Globals.Auth0Servers[indexPath.Row];
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
        /// Then the server is removed from the Webserver and the list.
        /// </summary>
        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        {
            try
            {
                Globals.HestiaWebServerInteractor.DeleteServer(Globals.Auth0Servers[indexPath.Row]);
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while deleting local server from Webserver");
                Console.Out.WriteLine(ex);
                WarningMessage.Display("Exception while deleting server", "An exception occurred while deleting a local server from the Webserver", owner);
            }

            // At this point, the deletion from the server has succeeded, so we can remove the server
            // from the Auth0Servers list. We delete the row from the list and then refresh the list. 
            // This order is necessary, preventing exceptions.
            Globals.Auth0Servers.RemoveAt(indexPath.Row);
            tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
            owner.RefreshServerList();
        }

        /// <summary>
        /// The action that is performed when the green plus icon is tapped to add a new server.
        /// It segues to the add Server screen.
        /// <see cref="AddServerViewController"/>
        /// </summary>
        public void InsertAction()
        {
            AddServerViewController serverView = owner.Storyboard.InstantiateViewController(strings.addServerViewController) as AddServerViewController;
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
        /// Is called after a press on edit button. The 'next' icon is changed to an insertion icon.
        /// </summary>
        public void WillBeginTableEditing(UITableView tableView)
        {
            owner.ReloadButtons(true);
        }

        /// <summary>
        /// Is called after a press on done button. The insertion icon is changed back to 'next' icon.
        /// </summary>
        public void DidFinishTableEditing(UITableView tableView)
        {
            owner.ReloadButtons(false);
        }
    }
}
