using Foundation;
using System;
using UIKit;

namespace Hestia
{


    public partial class UITableViewControllerLocalLogin : UITableViewController
    {
        string username = "admin";
        string password = "admin";

        public UITableViewControllerLocalLogin(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == "loginToConnect")
            {
                if (LoginUserName.Text == username && LoginPassword.Text == password)
                {
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