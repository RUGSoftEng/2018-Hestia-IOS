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
    [Register ("UITableViewControllerAccount")]
    partial class UITableViewControllerAccount
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel mailLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel phoneLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (mailLabel != null) {
                mailLabel.Dispose ();
                mailLabel = null;
            }

            if (nameLabel != null) {
                nameLabel.Dispose ();
                nameLabel = null;
            }

            if (phoneLabel != null) {
                phoneLabel.Dispose ();
                phoneLabel = null;
            }
        }
    }
}