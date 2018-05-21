using Foundation;
using System;
using UIKit;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.utils;
using Hestia.Resources;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerServerConnect : UITableViewController
    {
        NSUserDefaults userDefaults;
        string defaultServerName;
        string defaultIP;
        string defaultPort;
        const string ViewControllerTitle = "Server";

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
            defaultServerName = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(strings.defaultsPortHestia);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = ViewControllerTitle;

            if (defaultServerName != null)
            {
                newServerName.Text = defaultServerName;
            }
            if (defaultIP != null)
            {
                // Cut off the https:// or http://
                int index = defaultIP.LastIndexOf('/');
                if (index >= 0)
                {
                    defaultIP = defaultIP.Substring(index + 1);
                }
                newIP.Text = defaultIP;
            }
            if (defaultPort != null)
            {
                newPort.Text = defaultPort;
            }

            AssignReturnKeyBehaviour();
            newServerName.BecomeFirstResponder();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Console.WriteLine("Presented by" + PresentingViewController);
            if (PresentingViewController is UIViewControllerLocalGlobal)
            {
                SetCancelButtton();
            }
        }

        public void SetCancelButtton()
        {
            // Cancel button to go back to local/global screen
            UIBarButtonItem cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) => {
                DismissViewController(true, null);
            });
            NavigationItem.LeftBarButtonItem = cancel;
        }

        /// <summary>
        /// Assigns the return key behaviour.
        /// That is, go to the next field or connect in case of last field.
        /// </summary>
        /// See <see cref="TextFieldShouldReturn(UITextField)"/> for the behaviour
        void AssignReturnKeyBehaviour()
        {
            newServerName.ShouldReturn += TextFieldShouldReturn;
            newIP.ShouldReturn += TextFieldShouldReturn;
            newPort.ShouldReturn += TextFieldShouldReturn;

            newServerName.Tag = 1;
            newIP.Tag = 2;
            newPort.Tag = 3;
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == strings.segueToServerDiscovery)
            {
                return true;
            }

            bool validIp = false;

            try
            {
                validIp = PingServer.Check(strings.defaultPrefix + newIP.Text, int.Parse(newPort.Text));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                DisplayWarningMessage();
                return false;
            }

            if (validIp)
            {
                Globals.ServerName = newServerName.Text;
                Globals.IP = strings.defaultPrefix + newIP.Text;
                Globals.Port = int.Parse(newPort.Text);
                HestiaServerInteractor serverInteractor = new HestiaServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                Globals.LocalServerinteractor = serverInteractor;

                userDefaults.SetString(newServerName.Text, strings.defaultsServerNameHestia);
                userDefaults.SetString(Globals.IP, strings.defaultsIpHestia);
                userDefaults.SetString(newPort.Text, strings.defaultsPortHestia);

                return true;
            }

            DisplayWarningMessage();
            return false;
        }

        void DisplayWarningMessage()
        {
            string title = "Could not connect to server";
            string message = "Invalid server information";
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);

            connectButton.Selected = false;
        }

        /// <summary>
        /// Implements the behaviour of the return button
        /// </summary>
        /// <returns>Always <c>false</c>, because we do not want a line break</returns>
        /// <param name="textfield">Textfield.</param>
        bool TextFieldShouldReturn(UITextField textfield)
        {
            int nextTag = (int)textfield.Tag + 1;
            UIResponder nextResponder = View.ViewWithTag(nextTag);
            if (nextResponder != null)
            {
                nextResponder.BecomeFirstResponder();
            }
            else
            {
                // Remove keyboard, then connect
                textfield.ResignFirstResponder();
                if (ShouldPerformSegue(strings.serverConnectToDevicesSegue, this))
                {
                    PerformSegue(strings.serverConnectToDevicesSegue, this);
                }
            }
            return false;
        }
    }
}
