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
    [Register ("UITableViewControllerServer")]
    partial class UITableViewControllerServer
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ipLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel serverNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ipLabel != null) {
                ipLabel.Dispose ();
                ipLabel = null;
            }

            if (serverNameLabel != null) {
                serverNameLabel.Dispose ();
                serverNameLabel = null;
            }
        }
    }
}