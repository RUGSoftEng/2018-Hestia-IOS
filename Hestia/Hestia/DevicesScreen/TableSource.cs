using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
namespace Hestia.DevicesScreen
{
    public class TableSource : UITableViewSource
    {
        UITableViewControllerDevicesMain owner;

        List<string> TableItems;
        string CellIdentifier = "tableCell";

        public TableSource(string[] items)
        {
            TableItems = new List<string>(items);
        }

        public TableSource(string[] items, UITableViewControllerDevicesMain owner)
        {
            TableItems = new List<string>(items);
            //TableItems = items;
            this.owner = owner;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            string item = TableItems[indexPath.Row];
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }
      

            cell.TextLabel.Text = item;

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", TableItems[indexPath.Row], UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            owner.PresentViewController(okAlertController, true, null);
            tableView.DeselectRow(indexPath, true);
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true; // return false if you wish to disable editing for a specific indexPath or for all rows
        }
        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            return false; // return false if you don't allow re-ordering
        }
        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
      
            if (tableView.Editing)
            {
                if (indexPath.Row == tableView.NumberOfRowsInSection(0) - 1)
                    return UITableViewCellEditingStyle.Insert;
                else
                    return UITableViewCellEditingStyle.Delete;
            }
            else // not in editing mode, enable swipe-to-delete for all rows
                return UITableViewCellEditingStyle.Delete;
        }
        public void WillBeginTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();
            // insert the 'ADD NEW' row at the end of table display
            tableView.InsertRows(new NSIndexPath[] {
            NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
        }, UITableViewRowAnimation.Fade);
            // create a new item and add it to our underlying data (it is not intended to be permanent)
            TableItems.Add("(add new)");
            tableView.EndUpdates(); // applies the changes
        }
        public void DidFinishTableEditing(UITableView tableView)
        {
            tableView.BeginUpdates();
            // remove our 'ADD NEW' row from the underlying data
            TableItems.RemoveAt((int)tableView.NumberOfRowsInSection(0) - 1); // zero based :)
                                                                              // remove the row from the table display
            tableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(tableView.NumberOfRowsInSection(0) - 1, 0) }, UITableViewRowAnimation.Fade);
            tableView.EndUpdates(); // applies the changes
        }

    }
}
