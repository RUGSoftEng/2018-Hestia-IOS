using Foundation;
using ObjCRuntime;
using System;
using UIKit;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.utils;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerServerConnect : UITableViewController
    {
        NSUserDefaults userDefaults;

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            newServerName.ShouldReturn += TextFieldShouldReturn;
            newIP.ShouldReturn += TextFieldShouldReturn;
            newPort.ShouldReturn += TextFieldShouldReturn;

            newServerName.Tag = 1;
            newIP.Tag = 2;
            newPort.Tag = 3;

            newServerName.BecomeFirstResponder();

            userDefaults = NSUserDefaults.StandardUserDefaults;
            var defaultServerName = userDefaults.StringForKey(Resources.strings.defaultsServerNameHestia);
            if (defaultServerName != null)
            {
                newServerName.Text = defaultServerName;
                newServerName.Placeholder = defaultServerName;
            }

            var defaultIP = userDefaults.StringForKey(Resources.strings.defaultsIpHestia);
            if (defaultIP != null)
            {
                newIP.Text = defaultIP;
                newIP.Placeholder = defaultIP;
            }
            var defaultPort = userDefaults.StringForKey(Resources.strings.defaultsPortHestia);
            if (defaultPort != null)
            {
                newPort.Text = defaultPort;
                newPort.Placeholder = defaultPort;
            }
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            bool validIp = false;

            try
            {
                validIp = PingServer.Check(newIP.Text, int.Parse(newPort.Text));
            }
            catch (Exception exception)
            {
                Console.Write(exception.StackTrace);
                displayWarningMessage();
                return false;
            }
            if (validIp)
            {
                userDefaults.SetString(newServerName.Text, Resources.strings.defaultsServerNameHestia);
                userDefaults.SetString(newIP.Text, Resources.strings.defaultsIpHestia);
                userDefaults.SetString(newPort.Text, Resources.strings.defaultsPortHestia);

                Globals.ServerName = newServerName.Text;
                Globals.IP = newIP.Text;
                Globals.Port = int.Parse(newPort.Text);
                ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                Globals.LocalServerinteractor = serverInteractor;

                return true;
            }
            else
            {
                displayWarningMessage();
                return false;
            }
        }

        public void displayWarningMessage()
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = "Could not connect to server",
                Message = "Invalid server information"
            };
            alert.AddButton("OK");
            alert.Show();
            connectButton.Selected = false;
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
                if (ShouldPerformSegue(Resources.strings.serverConnectToDevicesSegue, this))
                {
                    PerformSegue(Resources.strings.serverConnectToDevicesSegue, this);
                }
            }
            return false; //No line-breaks.
        }
    }
}