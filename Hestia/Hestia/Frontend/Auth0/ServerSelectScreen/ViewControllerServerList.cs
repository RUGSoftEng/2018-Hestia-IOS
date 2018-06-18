using CoreGraphics;
using Hestia.Backend;
using Hestia.Backend.Exceptions;
using Hestia.Backend.Models;
using Hestia.Frontend.Resources;
using Hestia.Frontend.SettingsScreen;
using Hestia.Resources;
using System;
using System.Collections.Generic;
using UIKit;

namespace Hestia.Frontend.Auth0.ServerSelectScreen
{
    /// <summary>
    /// This ViewController contains the View that holds a list of local servers that are on the Webserver.
    /// The servers that the user wants to show up in the Devices main screen should be selected.
    /// The user can also add and delete servers, and change the name.
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
        // Edit button in top right (appears in edit mode)
        UIBarButtonItem done;

        public ViewControllerServerList(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            RefreshServerList();
            Title = strings.selectServerTitle;
            TableView.Source = new TableSourceServerList(this);

            Globals.DefaultLightGray = TableView.BackgroundColor;

            // Fix the bottom position of the view, such that icons appear at same place when reloaded.
            bottomOfView = TableView.Bounds.Bottom;

            TableView.TableFooterView = GetTableViewFooter();
            // To be able to tap a row in editing mode for changing name
            TableView.AllowsSelectionDuringEditing = true;

            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) =>
            {
                SetEditingState();
            });

            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) =>
            {
                CancelEditingState();
            });

            //Pull to refresh
            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += RefreshTable;
            TableView.Add(RefreshControl);

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = edit;
        }



        /// <summary>
        /// This method is called when the View will appear. It creates the cancel button and assigns it the correct behaviour,
        /// depending on the previous ViewController. If the View is shown in the global settings screen, the cancel button 
        /// should not appear. See, <see cref="SetCancelButtton()"/>.
        /// </summary>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            RefreshServerList();
            TableView.ReloadData();
            // Set cancel button if the Server connect screen is loaded from Appdelegate
            if (NavigationController.ViewControllers.Length < 2)
            {
                SetCancelButtton();
            }
            //Set cancel button otherwise if it does not appear in the Settings screen
            else if (!(NavigationController.ViewControllers[NavigationController.ViewControllers.Length - 2] is UITableViewControllerGlobalSettingsScreen))
            {
                SetCancelButtton();
            }
            ReloadButtons(TableView.Editing);
        }

        /// <summary>
        /// Sets the cancel buttton, depending on the ViewController that calls it. If the previous ViewController is the
        /// <see cref="UIViewControllerLocalGlobal"/>, the the cancel button should dismiss the ViewController. If the 
        /// ViewController is directly presented from the <see cref="AppDelegate"/>, a new <see cref="UIViewControllerLocalGlobal"/>
        /// should be instantiated.
        /// </summary>
        public void SetCancelButtton()
        {
            // Cancel button to go back to local/global screen
            cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) =>
            {
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
        /// This method is used in <see cref="ReloadButtons(bool)"/> to set the behaviour of the done button. 
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
        /// See, <see cref="TableSourceServerList.DidFinishTableEditing(UITableView)"/>
        /// </summary>
        public void CancelEditingState()
        {
            TableView.SetEditing(false, true);
            NavigationItem.RightBarButtonItem = edit;
            ((TableSourceServerList)TableView.Source).DidFinishTableEditing(TableView);
        }

        /// <summary>
        /// This methods is called if the edit button is touched. The button is changed to display "done" and the 
        /// list changes to editing mode (the delete icons appear).
        /// See, <see cref="TableSourceServerList.WillBeginTableEditing(UITableView))"/>
        /// </summary>
        public void SetEditingState()
        {
            ((TableSourceServerList)TableView.Source).WillBeginTableEditing(TableView);
            TableView.SetEditing(true, true);
            NavigationItem.RightBarButtonItem = done;
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
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting servers");
                Console.WriteLine(ex);
                WarningMessage.Display("Exception whle getting server", "Could not get the server information about local server from Auth0 server.", this);
            }

            TableView.Source = new TableSourceServerList(this);
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
            // Next / add device button
            UIButton button = new UIButton(UIButtonType.System);
            button.Frame = new CGRect(TableView.Bounds.Width - IconDimension - Padding, bottomOfView - TableViewFooterHeight - Padding, IconDimension, IconDimension);
            if (isEditing)
            {
                button.SetBackgroundImage(UIImage.FromBundle(strings.addDeviceIcon), UIControlState.Normal);
            }
            else
            {
                button.SetBackgroundImage(UIImage.FromBundle(strings.arrowRight), UIControlState.Normal);
            }

            button.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (isEditing)
                {
                    ((TableSourceServerList)TableView.Source).InsertAction();
                }
                else
                {
                    if (ShouldPerformSegue())
                    {
                        UIStoryboard devicesMainStoryboard = UIStoryboard.FromName(strings.devices2StoryBoard, null);
                        var devicesMain = devicesMainStoryboard.InstantiateViewController(strings.navigationControllerDevicesMain) as UINavigationController;
                        ShowViewController(devicesMain, this);
                    }
                }
            };

            ParentViewController.View.AddSubview(button);
        }

        /// <summary>
        /// The footer has enough free space to shown the complete next/insertion icons 
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
