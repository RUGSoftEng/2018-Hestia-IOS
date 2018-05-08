using Foundation;
using System;
using UIKit;
using Hestia.DevicesScreen.resources;

namespace Hestia
{


    public partial class UITableViewControllerLocalLogin : UITableViewController
    {
        NSUserDefaults userDefaults;

        public UITableViewControllerLocalLogin(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Globals.LocalLogin = true;
            Globals.DefaultLightGray = TableView.BackgroundColor;
            LoginUserName.ShouldReturn += TextFieldShouldReturn;
            LoginPassword.ShouldReturn += TextFieldShouldReturn;

            LoginUserName.Tag = 1;
            LoginPassword.Tag = 2;

            // Get Shared User Defaults
            LoginUserName.BecomeFirstResponder();
            userDefaults = NSUserDefaults.StandardUserDefaults;

            var defaultUserName = userDefaults.StringForKey(Resources.strings.defaultsUsernameHestia);
            if (defaultUserName != null)
            {
                LoginUserName.Text = defaultUserName;
                LoginUserName.Placeholder = defaultUserName;
            }

            var defaultPassWord = userDefaults.StringForKey(Resources.strings.defaultsPasswordHestia);
            if (defaultPassWord != null)
            {
                LoginPassword.Text = defaultPassWord;
                LoginPassword.Placeholder = defaultPassWord;
            }

        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == Resources.strings.loginToConnectSegue)
            {
                if (LoginUserName.Text == Resources.strings.username && LoginPassword.Text == Resources.strings.password)
                {

                    userDefaults.SetString(LoginUserName.Text, Resources.strings.defaultsUsernameHestia);
                    userDefaults.SetString(LoginPassword.Text, Resources.strings.defaultsUsernameHestia);
                    Globals.UserName = LoginUserName.Text;

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

        private bool TextFieldShouldReturn(UITextField textfield)
        {
            int nextTag = (int)textfield.Tag + 1;
            UIResponder nextResponder = this.View.ViewWithTag(nextTag);
            if (nextResponder != null)
            {
                nextResponder.BecomeFirstResponder();
            }
            else
            {
                // Remove keyboard, then connect
                textfield.ResignFirstResponder();
                if (ShouldPerformSegue(Resources.strings.loginToConnectSegue, this))
                {
                    PerformSegue(Resources.strings.loginToConnectSegue, this);
                }
            }
            return false; //No line-breaks.
        }
    }
}