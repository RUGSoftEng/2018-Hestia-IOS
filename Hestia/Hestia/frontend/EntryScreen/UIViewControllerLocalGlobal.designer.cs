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

namespace Hestia.Frontend.EntryScreen
{
    [Register ("UIViewControllerLocalGlobal")]
    partial class UIViewControllerLocalGlobal
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ToGlobalButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ToLocalButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ToGlobalButton != null) {
                ToGlobalButton.Dispose ();
                ToGlobalButton = null;
            }

            if (ToLocalButton != null) {
                ToLocalButton.Dispose ();
                ToLocalButton = null;
            }
        }
    }
}