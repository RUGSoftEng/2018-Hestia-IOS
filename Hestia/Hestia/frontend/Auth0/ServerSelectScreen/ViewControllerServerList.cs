using System;
using UIKit;
using Hestia.Auth0;

using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;

using CoreGraphics;
using Hestia.backend.models;
using Hestia.Resources;
using System.Collections.Generic;

namespace Hestia
{
    /// <summary>
    /// This ViewController contains the View that holds a list of local servers that are on the Webserver.
    /// The servers that the user wants to show up in the Devices main screen should be selected.
    /// See, <see cref="TableSourceServerList"/>.
    /// </summary>
    public partial class ViewControllerServerList : UITableViewController
    {
        const int TableViewFooterHeight = 50;
        const int IconDimension = 50;
        const int Padding = 15;
        nfloat bottomOfView;

        // Cancel button in top left 
        UIBarButtonItem cancel;
        // Edit button in top right (is shown initially)
        UIBarButtonItem edit;
        // Edit button in top right (appears in edit mode)lly)
        UIBarButtonItem done;

        public ViewControllerServerList(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = strings.selectServerTitle;
            TableView.Source = new TableSourceServerList(this);

            // Fix the bottom position of the view, such that icons appear at same place when reloaded.
            bottomOfView = TableView.Bounds.Bottom;

            TableView.TableFooterView = GetTableViewFooter();
           // RefreshServerList();
            // To be able to tap a row in editing mode for changing name
            TableView.AllowsSelectionDuringEditing = true;

            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                SetEditingState();
            });

            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                CancelEditingState();
            });

            //Pull to refresh
            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += RefreshTable;
            TableView.Add(RefreshControl);

            // Set right button initially to edit 
            NavigationItem.LeftBarButtonItem = edit;
            NavigationItem.RightBarButtonItem = cancel;
        }

        /// <summary>
        /// If the View appeared in the application, the actions of the done button are set. The done button leads
        /// to the Devices main screen.
        /// </summary>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            RefreshServerList();
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
            ReloadButtons(TableView.Editing);
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
                cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) => {
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

        /// <summary>
        /// This methods is called if the done button is touched. The button is changed to display "edit" and the 
        /// list changes to normal mode (the delete icons disappear).
        /// See, <see cref="TableSourceDevicesMain.DidFinishTableEditing(UITableView)"/>
        /// </summary>
        public void CancelEditingState()
        {
            TableView.SetEditing(false, true);
            NavigationItem.LeftBarButtonItem = edit;
            ((TableSourceServerList)TableView.Source).DidFinishTableEditing(TableView);
        }


        /// <summary>
        /// This methods is called if the edit button is touched. The button is changed to display "done" and the 
        /// list changes to editing mode (the delete icons appear).
        /// See, <see cref="TableSourceDevicesMain.WillBeginTableEditing(UITableView))"/>
        /// </summary>
        public void SetEditingState()
        {
            ((TableSourceServerList)TableView.Source).WillBeginTableEditing(TableView);
            TableView.SetEditing(true, true);
            NavigationItem.LeftBarButtonItem = done;
        }

        /// <summary>
        /// This method should be called if the list of devices should be updated from the server. 
        /// This method is called for example when pull-to-refresh is performed.
        /// </summary>
        public void RefreshServerList()
        {
            try
            {
                List<HestiaServer> servers = Globals.HestiaWebServerInteractor.GetServers();
                Globals.Auth0Servers = servers;
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting servers");
                Console.WriteLine(ex);
                WarningMessage.Display("Exception whle getting server", "Could not get the server information about local server from Auth0 server.", this);
            }

            TableView.Source = new TableSourceServerList(this);
        }

        void HandleException(TableSourceServerList source, ServerInteractionException ex)
        {
            Console.WriteLine("Exception while getting devices from local server");
            Console.WriteLine(ex);
            WarningMessage.Display("Could not refresh devices", "Exception while getting devices from local server", this);
           // source.serverDevices = new List<List<Device>>();
            TableView.ReloadData();
        }


        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            RemoveButtons();
        }

        void RemoveButtons()
        {
            foreach (UIView view in ParentViewController.View.Subviews)
            {
                if (view is UIButton)
                {
                    view.RemoveFromSuperview();
                }
            }
        }

        public void ReloadButtons(bool isEditing)
        {
            RemoveButtons();
            // Voice control / add device button
            UIButton button = new UIButton(UIButtonType.System);
            button.Frame = new CGRect(TableView.Bounds.Width - IconDimension - Padding, bottomOfView - TableViewFooterHeight - Padding, IconDimension, IconDimension);
            if (isEditing)
            {
                button.SetBackgroundImage(UIImage.FromBundle(strings.addDeviceIcon), UIControlState.Normal);
            }
            else
            {
                button.SetBackgroundImage(UIImage.FromBundle(strings.voiceControlIcon), UIControlState.Normal);
            }


            button.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (isEditing)
                {   // segue to add device
                    ((TableSourceServerList)TableView.Source).InsertAction();
                }
                else
                {
                    // segue
                }
            };


            ParentViewController.View.AddSubview(button);
        }

        /// <summary>
        /// The footer has enough free space to shown the complete microphone/insertion icons 
        /// if the user scrolls to the bottom of the page.
        /// </summary>
        /// <returns>The table view footer.</returns>
        public UIView GetTableViewFooter()
        {
            UIView view = new UIView(new CGRect(0, 0, TableView.Bounds.Width, TableViewFooterHeight));
            return view;
        }

        /// <summary>
        /// Method for Pull to refresh
        /// </summary>
        void RefreshTable(object sender, EventArgs e)
        {
            RefreshControl.BeginRefreshing();
            RefreshServerList();
            RefreshControl.EndRefreshing();
        }


    }
}

