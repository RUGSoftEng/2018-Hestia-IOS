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
        string serverNameHestia = "servernameHestia";
        string ipHestia = "ipHestia";
        string portHestia = "portHestia";
        NSUserDefaults userDefaults;

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            newServerName.BecomeFirstResponder();

            userDefaults = NSUserDefaults.StandardUserDefaults;
            var defaultServerName = userDefaults.StringForKey(serverNameHestia);
            if (defaultServerName != null)
            {
                newServerName.Text = defaultServerName;
                newServerName.Placeholder = defaultServerName;
            }

            var defaultIP = userDefaults.StringForKey(ipHestia);
            if (defaultIP != null)
            {
                newIP.Text = defaultIP;
                newIP.Placeholder = defaultIP;
            }
            var defaultPort = userDefaults.StringForKey(portHestia);
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
                userDefaults.SetString(newServerName.Text, serverNameHestia);
                userDefaults.SetString(newIP.Text, ipHestia);
                userDefaults.SetString(newPort.Text, portHestia);

                Globals.ServerName = newServerName.Text;
                Globals.IP = newIP.Text;
                Globals.Port = int.Parse(newPort.Text);
                ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                Globals.LocalServerInteractor = serverInteractor;

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
    }
}