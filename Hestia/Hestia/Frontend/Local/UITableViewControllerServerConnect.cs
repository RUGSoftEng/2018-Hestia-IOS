using Foundation;
using Hestia.Backend;
using Hestia.Backend.Utils;
using Hestia.Frontend.Resources;
using Hestia.Frontend.SettingsScreen;
using Hestia.Resources;
using System;
using UIKit;

namespace Hestia.Frontend.Local
{
    /// <summary>
    /// This class defines the ViewController that contains the Server Connect view, 
    /// that is enter the information of the local server. It is also used in the Settings screen.
    /// </summary>
    public partial class UITableViewControllerServerConnect : UITableViewController
    {
        NSUserDefaults userDefaults;
        string defaultServerName;
        string defaultIP;
        const string ViewControllerTitle = "Server";

        /// <summary>
        /// Constructor retrieves user defaults to display if this view appears in the Settings screen.
        /// </summary>
        /// <param name="handle"></param>
        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
            defaultServerName = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
        }

        /// <summary>
        /// This method is called if the View will appear and sets the title of the ViewController and 
        /// the placeholders in the inputfields. It places the cursor on the first input field and sets the 
        /// actions of the cancel button.
        /// </summary>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Title = ViewControllerTitle;

            if (defaultServerName != null)
            {
                newServerName.Text = defaultServerName;
            }
            if (defaultIP != null)
            {
                newIP.Text = defaultIP;
            }

            AssignReturnKeyBehaviour();
            // Place cursor in first input field
            newServerName.BecomeFirstResponder();

            // Set cancel button if the Server connect screen is loaded from Appdelegate
            if (NavigationController.ViewControllers.Length < 2)
            {
                SetCancelButtton();
            }
            // Set cancel button otherwise if it does not appear in the Settings screen
            else if (!(NavigationController.ViewControllers[NavigationController.ViewControllers.Length - 2] is UITableViewControllerLocalSettingsScreen))
            {
                SetCancelButtton();
            }
        }

        public void SetCancelButtton()
        {
            // Cancel button to go back to local/global screen
            UIBarButtonItem cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) => {
                // If loaded from Appdelegate, instantiate the Local/Global screen
                if (PresentingViewController is null)
                {
                    var initialViewController = AppDelegate.mainStoryboard.InstantiateInitialViewController();
                    PresentViewController(initialViewController, true, null);
                }
                else
                {   // Called from local/global screen
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

        /// <summary>
        /// Determine if the next screen should be loaded. Server discovery should always be loaded. 
        /// The devices main screen should only be loaded if the Server is valid.
        /// </summary>
        /// <param name="segueIdentifier"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if (segueIdentifier == strings.segueToServerDiscovery)
            {
                return true;
            }

            bool validIp = false;

            try
            {
                string address = strings.defaultPrefix + newIP.Text + ":" + int.Parse(strings.defaultPort);
                validIp = PingServer.Check(address);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                WarningMessage.Display("Could not connect to server", "Invalid server information", this);
                connectButton.Selected = false;
                return false;
            }

            if (validIp)
            {
                Globals.ServerName = newServerName.Text;
                Globals.Address = strings.defaultPrefix + newIP.Text + ":" + strings.defaultPort;
                HestiaServerInteractor serverInteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address));
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
