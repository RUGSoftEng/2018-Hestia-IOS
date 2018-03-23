using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using System.Drawing;
using System.Collections;

namespace Hestia.DevicesScreen
{
    public class TableSource : UITableViewSource
    {
        UITableViewControllerDevicesMain owner;

        // Use a Hashtable to hold the switches - this means that when a switch is created for a row which may have previously been
        // displayed, we know we're replacing the old one because the Hashtable is keyed on row.
        Hashtable Switches = new Hashtable();

        //List<string> TableItems;
        List<DataItem> TableItems;
        string CellIdentifier = "tableCell";

        // Declare a delegate to handle switch stat changes
        public delegate void mySwitchHandler(object sender, MyEventArgs e);

        public TableSource(List<DataItem> items)
        {
            //TableItems = new List<string>(items);
            TableItems = items;
        }

        public TableSource(List<DataItem> items, UITableViewControllerDevicesMain owner)
        {
            //TableItems = new List<string>(items);
            TableItems = items;
            //TableItems = items;
            this.owner = owner;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

           // string item = TableItems[indexPath.Row];
            // request a recycled cell to save memory
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
            }
      
            // Create a new UISwitch and set up its delegate for the value changing
            UISwitch MySwitch = new UISwitch();
            MySwitch.ValueChanged += delegate (object sender, EventArgs e)
            {
                MyEventArgs myE = new MyEventArgs();
                myE.SwitchState = MySwitch.On;
                myE.indexPath = indexPath;
                HandleSwitchChanged(this, myE);
            };

            cell.TextLabel.Text = TableItems[(indexPath.Row)].Label;;
            // Set the switch's state to that of the appropriate data item.
            MySwitch.On = TableItems[(indexPath.Row)].State;

            // Replace the cell's AccessoryView with the new UISwitch
            cell.AccessoryView = MySwitch;

            // Keep a reference to the UISwitch - note using a Hashtable to ensure we only have one for any given row
            Switches[indexPath.Row] = cell.AccessoryView;


            return cell;
        }

        // Handler for switch changed events
        protected void HandleSwitchChanged(object sender, MyEventArgs e)
        {
            TableItems[e.indexPath.Row].State = e.SwitchState;
            //                UIAlertView Alert = new UIAlertView();
            //                Alert.Title = "Changed";
            //                Alert.Message = string.Format("Switch on row {0} has changed", e.indexPath.Row);
            //                Alert.AddButton("Ok");
            //                Alert.Show();

        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", TableItems[indexPath.Row].Label, UIAlertControllerStyle.Alert);
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
            TableItems.Add(new DataItem() { Label = " Device 3", State = false });

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
