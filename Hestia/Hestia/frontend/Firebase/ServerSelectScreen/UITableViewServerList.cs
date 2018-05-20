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
            Title = Hestia.Resources.strings.selectServerTitle;
            TableView.Source = new TableSourceServerList();

            // The done button that loads the devicesMainScreen with the selected servers
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                if (ShouldPerformSegue())
                {
                    UIStoryboard devicesMainStoryboard = UIStoryboard.FromName("Devices2", null);

                    var devicesMain = devicesMainStoryboard.InstantiateViewController("navigationDevicesMain") as UINavigationController;
                    ShowViewController(devicesMain, this);
                }
            });
            NavigationItem.RightBarButtonItem = done;
        }

        bool ShouldPerformSegue()
        {
            foreach (HestiaServerInteractor interactor in Globals.GetSelectedServers())
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
