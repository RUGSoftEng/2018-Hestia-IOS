using Foundation;
using System;
using UIKit;

namespace Hestia
{


    public partial class UITableViewControllerLocalLogin : UITableViewController
    {
        string username = "admin";
        string password = "admin";
        string usernameHestia = "usernameHestia";
        string passwordHestia = "passwordHestia";
        NSUserDefaults userDefaults;
        string loginToConnectSegue = "loginToConnect";

        public UITableViewControllerLocalLogin(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Get Shared User Defaults
            LoginUserName.BecomeFirstResponder();
            userDefaults = NSUserDefaults.StandardUserDefaults;

            var defaultUserName = userDefaults.StringForKey(usernameHestia);
            if (defaultUserName != null)
            {
                LoginUserName.Text = defaultUserName;
                LoginUserName.Placeholder = defaultUserName;
            }

            var defaultPassWord = userDefaults.StringForKey(passwordHestia);
            if (defaultPassWord != null)
            {
                LoginPassword.Text = defaultPassWord;
                LoginPassword.Placeholder = defaultPassWord;
            }

        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == loginToConnectSegue)
            {
                if (LoginUserName.Text == username && LoginPassword.Text == password)
                {
                    userDefaults.SetString(LoginUserName.Text, usernameHestia);
                    userDefaults.SetString(LoginPassword.Text, usernameHestia);
                    return true;
                }
                else
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "Invalid credentials",
                        Message = "Tip: use admin/admin"
                    };
                    alert.AddButton("OK");
                    alert.Show();
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}