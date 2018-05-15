using Foundation;
using System;
using UIKit;

using System.Drawing;
using System.Collections.Generic;
using System.Collections;
using CoreGraphics;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.exceptions;

namespace Hestia.DevicesScreen.EditDevice
{
    public class UIViewControllerEditDeviceName : UIViewController
    {
        UITableViewControllerDevicesMain owner;

        public UIViewControllerEditDeviceName(UITableViewControllerDevicesMain owner)
        {
            this.owner = owner;
        }

        public Device device;
        UIBarButtonItem saveName;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Rectangular cell displaying the label and inputfield
            UIView rectangle = new UIView(new CGRect(0, 90, View.Bounds.Width, 50));
            rectangle.BackgroundColor = UIColor.White;
            View.AddSubview(rectangle);

            // Label
            UILabel newName = new UILabel();
            newName.Text = "New name:";
            newName.Frame = new CGRect(15, 10, 100, 31);
            rectangle.AddSubview(newName);

            // Inputfield
            UITextField changeNameField = new UITextField();
            changeNameField.Frame = new CGRect(110, 10, View.Bounds.Width - 125, 31);
            changeNameField.Placeholder = device.Name;
            rectangle.AddSubview(changeNameField);


            // Rectangular cell displaying the label and inputfield
            UIView rectangle2 = new UIView(new CGRect(0, 140, View.Bounds.Width, 50));
            rectangle2.BackgroundColor = UIColor.White;
            View.AddSubview(rectangle2);

            // Label new IP
            UILabel newIP = new UILabel();
            newIP.Text = "New IP:";
            newIP.Frame = new CGRect(15, 10, 100, 31);
            rectangle2.AddSubview(newIP);

            // Inputfield
            UITextField changeIPField = new UITextField();
            changeIPField.Frame = new CGRect(110, 10, View.Bounds.Width - 125, 31);
            changeIPField.Placeholder = device.NetworkHandler.Ip;
            rectangle2.AddSubview(changeIPField);


            // Rectangular cell displaying the label and inputfield
            UIView rectangle3 = new UIView(new CGRect(0, 190, View.Bounds.Width, 50));
            rectangle3.BackgroundColor = UIColor.White;
            View.AddSubview(rectangle3);

            // Label new Port
            UILabel newPort = new UILabel();
            newPort.Text = "New Port:";
            newPort.Frame = new CGRect(15, 10, 100, 31);
            rectangle3.AddSubview(newPort);

            // Inputfield
            UITextField changePortField = new UITextField();
            changePortField.Frame = new CGRect(110, 10, View.Bounds.Width - 125, 31);
            changePortField.Placeholder = device.NetworkHandler.Port.ToString();
            rectangle3.AddSubview(changePortField);

            View.BackgroundColor = Globals.DefaultLightGray;
            Title = device.Name;

            // Save button
            saveName = new UIBarButtonItem(UIBarButtonSystemItem.Save, (s, e) => {
                if(changeNameField.Text.Length <= 0 || changeIPField.Text.Length <= 0 || changePortField.Text.Length <= 0)
                {
                    var alert = UIAlertController.Create("Error!", "You have to fill all the specifications.", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                else
                {
                    try
                    {
                        device.Name = changeNameField.Text;
                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while changing device name");
                        Console.WriteLine(ex.ToString());
                    }
                    device.NetworkHandler.Ip = changeIPField.Text;
                    device.NetworkHandler.Port = int.Parse(changePortField.Text); 
                }
                // Reset editing mode to be able to correctly update cell contents
                owner.CancelEditingState();

                owner.RefreshDeviceList();
                owner.SetEditingState();
                NavigationController.PopViewController(true);
            });
            NavigationItem.RightBarButtonItem = saveName;
        }
    }
}
