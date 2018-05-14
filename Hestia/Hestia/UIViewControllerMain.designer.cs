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
    [Register ("UIViewControllerMain")]
    partial class UIViewControllerMain
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LocalSignInButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LocalSignInButton != null) {
                LocalSignInButton.Dispose ();
                LocalSignInButton = null;
            }
        }
    }
}