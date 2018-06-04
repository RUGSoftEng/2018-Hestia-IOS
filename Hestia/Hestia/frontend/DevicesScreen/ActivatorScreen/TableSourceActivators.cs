using System;
using System.Collections;
using UIKit;
using Hestia.backend.models;
using Hestia.backend.exceptions;
using System.Collections.Generic;
using Foundation;
using Hestia.frontend;

namespace Hestia.DevicesScreen.ActivatorScreen
{
    public class TableSourceActivators : UITableViewSource
    {
        Device device;
        List<backend.models.Activator> activators;
        Hashtable actAccessories = new Hashtable();
        UITableViewActivators owner;

        public TableSourceActivators(Device device, UITableViewActivators owner)
        {
            this.device = device;
            this.owner = owner;
            activators = device.Activators;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
		{
            return activators.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            UITableViewCell cell = tableView.DequeueReusableCell(Resources.strings.activatorCell);
            backend.models.Activator act = activators[indexPath.Row];
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, Resources.strings.activatorCell);
            }
            if (act.State.Type == "bool")
            {
                UISwitch DeviceSwitch = new UISwitch();

                // Set the switch's state to that of the device.
                DeviceSwitch.On = (bool)act.State.RawState;
                DeviceSwitch.TouchUpInside += delegate
                {
                    try 
                    {
                        act.State = new ActivatorState(DeviceSwitch.On, "bool");
                    } 
                    catch(ServerInteractionException ex) 
                    {
                        Console.WriteLine("Exception while changing activator state");
                        Console.WriteLine(ex);
                        WarningMessage.Display("Could not set activator", "An exception occurred when changing the state of the activator on the server", owner);

                        // Reset back the switch
                        DeviceSwitch.On = (bool)act.State.RawState;
                    }
                };
                // Replace the cell's AccessoryView with the new UISwitch
                cell.AccessoryView = DeviceSwitch;

                // Keep a reference to the UISwitch - note using a Hashtable to ensure
                // we only have one for any given row
                actAccessories[indexPath.Row] = cell.AccessoryView;
            }
            else if (act.State.Type == "float")
            {
                UISlider slider = new UISlider();

                // Set the switch's state to that of the device.
                slider.Value = (float)act.State.RawState;
                slider.TouchUpInside += delegate
                {
                    try
                    {
                        act.State = new ActivatorState(slider.Value, "float");
                    } 
                    catch(ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while changing activator state");
                        Console.WriteLine(ex);
                        WarningMessage.Display("Could not set activator", "An exception occurred when changing the state of the activator on the server", owner);

                        // Reset back the slider
                        slider.Value = (float)act.State.RawState;
                    }   
                };

                // Replace the cell's AccessoryView with the new UISwitch
                cell.AccessoryView = slider;

                // Keep a reference to the UISwitch - note using a Hashtable to ensure
                // we only have one for any given row
                actAccessories[indexPath.Row] = cell.AccessoryView;
            }
            cell.TextLabel.Text = act.Name;

            return cell;
		}
	}
}
