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
    [Register ("UITableViewControllerLocalSettingsScreen")]
    partial class UITableViewControllerLocalSettingsScreen
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel serverName { get; set; }

        [Action ("UIButton87850_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton87850_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (serverName != null) {
                serverName.Dispose ();
                serverName = null;
            }
        }
    }
}