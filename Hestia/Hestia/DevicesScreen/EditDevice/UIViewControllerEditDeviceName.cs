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
            UIView rectangle = new UIView(new CGRect(0, 80, this.View.Bounds.Width, 50));
            rectangle.BackgroundColor = UIColor.White;
            this.View.AddSubview(rectangle);

            // Label
            UILabel newName = new UILabel();
            newName.Text = "New name:";
            newName.Frame = new CGRect(15, 10, 100, 31);
            rectangle.AddSubview(newName);

            // Inputfield
            UITextField changeNameField = new UITextField();
            changeNameField.Frame = new CGRect(110, 10, this.View.Bounds.Width - 125, 31);
            changeNameField.Placeholder = device.Name;
            rectangle.AddSubview(changeNameField);

            this.View.BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1.0f);
            Title = device.Name;


            // Save button
            saveName = new UIBarButtonItem(UIBarButtonSystemItem.Save, (s, e) => {
                device.Name = changeNameField.Text;
                owner.refreshDeviceList();
                this.NavigationController.PopViewController(true);
            });

            NavigationItem.RightBarButtonItem = saveName;

        }


    }
}



