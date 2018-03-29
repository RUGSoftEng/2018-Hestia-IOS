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

namespace Hestia.DevicesScreen
{
    [Register ("SettingsController")]
    partial class SettingsController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel serverName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (serverName != null) {
                serverName.Dispose ();
                serverName = null;
            }
        }
    }
}