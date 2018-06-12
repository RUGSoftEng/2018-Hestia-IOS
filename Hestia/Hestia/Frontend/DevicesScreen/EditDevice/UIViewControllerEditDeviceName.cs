using System;
using UIKit;
using CoreGraphics;

using Hestia.Backend.Models;
using Hestia.Backend.Exceptions;
using Hestia.Frontend.Resources;

namespace Hestia.Frontend.DevicesScreen.EditDevice
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

            View.BackgroundColor = Globals.DefaultLightGray;
            Title = device.Name;

            // Save button
            saveName = new UIBarButtonItem(UIBarButtonSystemItem.Save, (s, e) => {
                if(changeNameField.Text.Length <= 0 )
                {
                    WarningMessage.Display("Error", "You have to give a name for the device.", this);  
                }
                else
                {
                    try
                    {
                        device.Name = changeNameField.Text;
                        // Reset editing mode to be able to correctly update cell contents
                        owner.CancelEditingState();

                        owner.RefreshDeviceList();
                        owner.SetEditingState();
                        NavigationController.PopViewController(true);
                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while changing device name");
                        Console.WriteLine(ex);
                        WarningMessage.Display("Exception", "An exception occurred on the server when changing the name of the device", this);  
                    } 
                }
            });
            NavigationItem.RightBarButtonItem = saveName;
        }
    }
}
