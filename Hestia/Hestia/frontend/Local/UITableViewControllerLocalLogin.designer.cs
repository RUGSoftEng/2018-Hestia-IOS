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

namespace Hestia
{
    [Register ("UITableViewControllerLocalLogin")]
    partial class UITableViewControllerLocalLogin
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell connectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LoginPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LoginUserName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (connectButton != null) {
                connectButton.Dispose ();
                connectButton = null;
            }

            if (LoginPassword != null) {
                LoginPassword.Dispose ();
                LoginPassword = null;
            }

            if (LoginUserName != null) {
                LoginUserName.Dispose ();
                LoginUserName = null;
            }
        }
    }
}