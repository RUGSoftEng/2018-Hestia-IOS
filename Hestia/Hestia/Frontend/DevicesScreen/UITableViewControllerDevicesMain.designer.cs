﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hestia.Frontend.DevicesScreen
{
    [Register ("UITableViewControllerDevicesMain")]
    partial class UITableViewControllerDevicesMain
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem DevicesMainNavBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView DevicesTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem SettingsButton { get; set; }

        [Action ("SettingsButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SettingsButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (DevicesMainNavBar != null) {
                DevicesMainNavBar.Dispose ();
                DevicesMainNavBar = null;
            }

            if (DevicesTable != null) {
                DevicesTable.Dispose ();
                DevicesTable = null;
            }

            if (SettingsButton != null) {
                SettingsButton.Dispose ();
                SettingsButton = null;
            }
        }
    }
}