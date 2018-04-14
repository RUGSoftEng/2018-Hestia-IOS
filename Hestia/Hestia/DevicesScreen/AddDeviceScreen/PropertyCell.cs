using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;

using CoreGraphics;


namespace Hestia.DevicesScreen.AddDeviceScreen
{
    public class PropertyCell : UITableViewCell
    {
        public UITextField inputField;
        public PropertyCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);

            inputField = new UITextField()
            {
                
            };

            ContentView.AddSubviews(new UIView[] {inputField});

        }
        public void UpdateCell(string placeholder)
        {
            inputField.Placeholder = placeholder;
                       
        }
        public override void LayoutSubviews()
        {
           base.LayoutSubviews();
            inputField.Frame = new CGRect(15, 4, ContentView.Bounds.Width - 30, ContentView.Bounds.Height - 8);
        }
    }
}
