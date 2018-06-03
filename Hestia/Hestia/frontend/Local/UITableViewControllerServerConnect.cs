using Foundation;
using System;
using UIKit;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.utils;
using Hestia.Resources;
using Hestia.frontend;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerServerConnect : UITableViewController
    {
        NSUserDefaults userDefaults;
        string defaultServerName;
        string defaultIP;
        const string ViewControllerTitle = "Server";

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
            defaultServerName = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = ViewControllerTitle;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (defaultServerName != null)
            {
                newServerName.Text = defaultServerName;
            }
            if (defaultIP != null)
            {
                newIP.Text = defaultIP;
            }

            AssignReturnKeyBehaviour();
            newServerName.BecomeFirstResponder();

            if (NavigationController.ViewControllers.Length < 2)
            {
                SetCancelButtton();
            }
            else if (!(NavigationController.ViewControllers[NavigationController.ViewControllers.Length - 2] is UITableViewControllerLocalSettingsScreen))
            {
                SetCancelButtton();
            }
        }

        public void SetCancelButtton()
        {
            // Cancel button to go back to local/global screen
            UIBarButtonItem cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) => {
                if (PresentingViewController is null)
                {
                    var initialViewController = AppDelegate.mainStoryboard.InstantiateInitialViewController();
                    PresentViewController(initialViewController, true, null);
                }
                else
                {
                    DismissViewController(true, null);
                }
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

            newServerName.Tag = 1;
            newIP.Tag = 2;
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
                validIp = PingServer.Check(strings.defaultPrefix + newIP.Text, int.Parse(strings.defaultPort));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                WarningMessage.Display("Could not connect to server", "Invalid server information", this);
                connectButton.Selected = false;
                return false;
            }

            if (validIp)
            {
                Globals.ServerName = newServerName.Text;
                Globals.Address = strings.defaultPrefix + newIP.Text;
                HestiaServerInteractor serverInteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address, int.Parse(strings.defaultPort)));
                Globals.LocalServerinteractor = serverInteractor;

                userDefaults.SetString(newServerName.Text, strings.defaultsServerNameHestia);
                userDefaults.SetString(newIP.Text, strings.defaultsIpHestia);

                return true;
            }

            WarningMessage.Display("Could not connect to server", "Invalid server information", this);
            connectButton.Selected = false;
            return false;
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
