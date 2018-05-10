using Foundation;
using System;
using UIKit;
using Hestia.Resources;

namespace Hestia
{
    public partial class UIViewControllerAuth0 : UIViewController
    {
        NSUserDefaults userDefaults;

        public UIViewControllerAuth0 (IntPtr handle) : base (handle)
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Task<LoginResult> loginResult = GetLoginResult();

            //userDefaults.SetString(loginResult.IdentityToken, strings.defaultsIdentityTokenHestia);
            //userDefaults.SetString(loginResult.AccessToken), strings.defaultsAccessTokenHestia);
        
            // PerformSegue...
        }
    }
}
