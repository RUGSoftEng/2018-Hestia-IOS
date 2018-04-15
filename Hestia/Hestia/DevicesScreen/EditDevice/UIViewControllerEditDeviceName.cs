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
        public UIViewControllerEditDeviceName()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UIView rectangle = new UIView(new CGRect(0, 70, this.View.Bounds.Width, 50));
            rectangle.BackgroundColor = UIColor.Cyan;
            this.View.AddSubview(rectangle);

            UILabel newName = new UILabel();
            newName.Text = "New name:";
            newName.Frame = new CGRect(15, 10, 100, 31);
            rectangle.AddSubview(newName);

            UITextField changeNameField = new UITextField();
            changeNameField.Frame = new CGRect(110, 10, this.View.Bounds.Width - 125, 31);
            changeNameField.Placeholder = "test";
            rectangle.AddSubview(changeNameField);

            View.BackgroundColor = UIColor.LightGray;
            Title = "Edit name";


        }

    }
}



