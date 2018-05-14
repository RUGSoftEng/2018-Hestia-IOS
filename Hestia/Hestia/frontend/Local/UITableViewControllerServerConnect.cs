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
        string defaultServerName;
        string defaultIP;
        string defaultPort;

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
            defaultServerName = userDefaults.StringForKey(Resources.strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(Resources.strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(Resources.strings.defaultsPortHestia);
        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (defaultServerName != null)
            {
                newServerName.Text = defaultServerName;
            }
            if (defaultIP != null)
            {
                newIP.Text = defaultIP;
            }
            if (defaultPort != null)
            {
                newPort.Text = defaultPort;
            }

            newServerName.ShouldReturn += TextFieldShouldReturn;
            newIP.ShouldReturn += TextFieldShouldReturn;
            newPort.ShouldReturn += TextFieldShouldReturn;

            newServerName.Tag = 1;
            newIP.Tag = 2;
            newPort.Tag = 3;

            newServerName.BecomeFirstResponder();
        }


		public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            bool validIp = false;

            validIp = PingServer.Check(newIP.Text, int.Parse(newPort.Text));
          
            if (validIp)
            {
                userDefaults.SetString(newServerName.Text, Resources.strings.defaultsServerNameHestia);
                userDefaults.SetString(newIP.Text, Resources.strings.defaultsIpHestia);
                userDefaults.SetString(newPort.Text, Resources.strings.defaultsPortHestia);

                Globals.ServerName = newServerName.Text;
                Globals.IP = newIP.Text;
                Globals.Port = int.Parse(newPort.Text);
                HestiaServerInteractor serverInteractor = new HestiaServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                Globals.LocalServerinteractor = serverInteractor;

                return true;
            }
            else
            {
                DisplayWarningMessage();
                return false;
            }
        }


        private void DisplayWarningMessage()
        {
            string title = "Could not connect to server";
            string message = "Invalid server information";
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);

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
