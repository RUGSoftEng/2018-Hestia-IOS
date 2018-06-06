using System;
using UIKit;
using Hestia.Auth0;

using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;

namespace Hestia
{
    /// <summary>
    /// This ViewController contains the View that holds a list of local servers that are on the Webserver.
    /// The servers that the user wants to show up in the Devices main screen should be selected.
    /// See, <see cref="TableSourceServerList"/>.
    /// </summary>
    public partial class ViewControllerServerList : UITableViewController
    {
        UIBarButtonItem done;

        public ViewControllerServerList(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = Resources.strings.selectServerTitle;
            TableView.Source = new TableSourceServerList(this);
        }

        /// <summary>
        /// If the View appeared in the application, the actions of the done button are set. The done button leads
        /// to the Devices main screen.
        /// </summary>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TableView.ReloadData();
            // The done button that loads the devicesMainScreen with the selected servers
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                if (ShouldPerformSegue())
                {
                    UIStoryboard devicesMainStoryboard = UIStoryboard.FromName(Resources.strings.devices2StoryBoard, null);
                    var devicesMain = devicesMainStoryboard.InstantiateViewController(Resources.strings.navigationControllerDevicesMain) as UINavigationController;
                    ShowViewController(devicesMain, this);
                }
            });
            NavigationItem.RightBarButtonItem = done;
        }

        /// <summary>
        /// This method is called when the View will appear. It creates the cancel button and assigns it the correct behaviour,
        /// depending on the previous ViewController. If the View is shown in the global settings screen, the cancel button 
        /// should not appear. See, <see cref="SetCancelButtton(UIViewController)"/>.
        /// </summary>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!(PresentingViewController is UINavigationController))
            {
                SetCancelButtton(PresentingViewController);
            }
        }

        /// <summary>
        /// Sets the cancel buttton, depending on the ViewController that calls it. If the previous ViewController is the
        /// <see cref="UIViewControllerLocalGlobal"/>, the the cancel button should dismiss the ViewController. If the 
        /// ViewController is directly presented from the <see cref="AppDelegate"/>, a new <see cref="UIViewControllerLocalGlobal"/>
        /// should be instantiated.
        /// </summary>
        /// <param name="uIViewController">The ViewController from which the ServeSelect list is called.</param>
        public void SetCancelButtton(UIViewController uIViewController)
        {
            // Cancel button to go back to local/global screen
            UIBarButtonItem cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) => {
                if (uIViewController is UIViewControllerLocalGlobal)
                {
                    DismissViewController(true, null);
                }
                // Create new local/global screen
                UIStoryboard devicesMainStoryboard = UIStoryboard.FromName(Resources.strings.mainStoryBoard, null);
                var localGlobal = devicesMainStoryboard.InstantiateInitialViewController();
                PresentViewController(localGlobal, true, null);
            });
            NavigationItem.LeftBarButtonItem = cancel;
        }

        /// <summary>
        /// This method is used in <see cref="ViewDidAppear(bool)"/> to set the behaviour of the done button. 
        /// The next screen should only be loaded if the local servers can be used without exceptions, which is 
        /// checked by this mehtod.
        /// </summary>
        /// <returns>True, if the servers can be used safely, false otherwise.</returns>
        bool ShouldPerformSegue()
        {
            foreach (HestiaServerInteractor interactor in Globals.GetInteractorsOfSelectedServers())
            {
                try
                {
                    interactor.GetDevices();
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while getting devices from local server");
                    Console.WriteLine(ex);
                    WarningMessage.Display("Could not fetch devices", "The server information is invalid. Did you forget to include https://?", this);
                    return false;
                }
            }
            return true;
        }
    }
}
