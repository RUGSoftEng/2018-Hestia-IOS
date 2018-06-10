// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hestia.Frontend.Local
{
    [Register ("UITableViewControllerServerConnect")]
    partial class UITableViewControllerServerConnect
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell connectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField newIP { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField newServerName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (connectButton != null) {
                connectButton.Dispose ();
                connectButton = null;
            }

            if (newIP != null) {
                newIP.Dispose ();
                newIP = null;
            }

            if (newServerName != null) {
                newServerName.Dispose ();
                newServerName = null;
            }
        }
    }
}