using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.utils;
using Hestia.backend.exceptions;

namespace Hestia
{
    public partial class UITableViewServerList : UITableViewController
    {
        UIBarButtonItem done;

        public UITableViewServerList (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = Resources.strings.selectServerTitle;
            TableView.Source = new TableSourceServerList();

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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Console.WriteLine("Presented by" + PresentingViewController);
            if (!(PresentingViewController is UINavigationController))
            {
                SetCancelButtton(PresentingViewController);
            }
        }

        /// <summary>
        /// Sets the cancel buttton, depending on the ViewController that calls it.
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
                UIStoryboard devicesMainStoryboard = UIStoryboard.FromName(Resources.strings.mainStoryBoard, null);
                var localGlobal = devicesMainStoryboard.InstantiateInitialViewController();
                PresentViewController(localGlobal, true, null);
            });
            NavigationItem.LeftBarButtonItem = cancel;
        }

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
                    DisplayWarningMessage();
                    return false;
                }
            }
            return true;
        }

        void DisplayWarningMessage()
        {
            string title = "Could not fetch devices";
            string message = "The server information is invalid. Did you forget to include https://?";
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
        }
    }
}
