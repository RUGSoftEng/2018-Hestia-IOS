using System;
using UIKit;
using System.Collections.Generic;

using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.backend;
using Hestia.Resources;
using CoreGraphics;
using Hestia.frontend;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
        const int TableViewHeaderHeight = 35;
        const int TableViewHeaderTopPadding = 5;
        const int IconDimension = 50;

       // Done button in top right (appears in edit mode)
        UIBarButtonItem done;
        // Edit button in top right (is shown initially)
        UIBarButtonItem edit;

        List<Device> devices = new List<Device>();

        // Constructor
        public UITableViewControllerDevicesMain(IntPtr handle) : base(handle)
        {
        }

        public void CancelEditingState()
        {
            DevicesTable.SetEditing(false, true);
            NavigationItem.LeftBarButtonItem = edit;
            ((TableSourceDevicesMain)DevicesTable.Source).DidFinishTableEditing(DevicesTable);
        }

        public void SetEditingState()
        {
            ((TableSourceDevicesMain)DevicesTable.Source).WillBeginTableEditing(DevicesTable);
            DevicesTable.SetEditing(true, true);
            NavigationItem.LeftBarButtonItem = done;
        }

        public void RefreshDeviceList()
        {
            TableSourceDevicesMain source = new TableSourceDevicesMain(this);
            source.serverDevices = new List<List<Device>>();
            if (Globals.LocalLogin)
            {
                source.numberOfServers = int.Parse(Resources.strings.defaultNumberOfServers);
                source.serverDevices.Add(Globals.GetDevices());
            }
            else
            {
                source.numberOfServers = Globals.GetNumberOfSelectedServers();
                foreach (HestiaServerInteractor interactor in Globals.GetInteractorsOfSelectedServers())
                {
                    try
                    {
                        source.serverDevices.Add(interactor.GetDevices());
                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while getting devices from local server");
                        Console.WriteLine(ex);
                        WarningMessage message = new WarningMessage("Could not refresh devices", "Exception while getting devices from local server, through Auth0 server", this);

                    }
                }
            }
            DevicesTable.Source = source;
        }

        public UIView GetTableViewHeader(bool isEditing)
        {
            UIView view = new UIView(new CGRect(0, 0, TableView.Bounds.Width, TableViewHeaderHeight));

            // Voice control / add device button
            UIButton button = new UIButton(UIButtonType.System);
            button.Frame = new CGRect(TableView.Bounds.Width / 2 - IconDimension / 2, TableViewHeaderTopPadding, IconDimension, IconDimension);
            if (isEditing)
            {
                button.SetBackgroundImage(UIImage.FromBundle(strings.addDeviceIcon), UIControlState.Normal);
            }
            else
            {
                button.SetBackgroundImage(UIImage.FromBundle(strings.voiceControlIcon), UIControlState.Normal);
            }

            button.TouchUpInside += delegate {
                if (isEditing)
                { // segue to add device
                    ((TableSourceDevicesMain)DevicesTable.Source).InsertAction();
                }
                else
                {
                    // activate voice control
                }
            };

            view.AddSubview(button);
            return view;
        }

		public override void ViewDidLoad()
        { 
            base.ViewDidLoad();

            // Get the voice control button
            DevicesTable.TableHeaderView = GetTableViewHeader(false);
    
            RefreshDeviceList();
            Globals.DefaultLightGray = TableView.BackgroundColor;
            // To be able to tap row in editing mode for changing name
            DevicesTable.AllowsSelectionDuringEditing = true;  

            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                CancelEditingState();
            });

            edit = new UIBarButtonItem(UIBarButtonSystemItem.Edit, (s, e) => {
                SetEditingState();
            });

            //Pull to refresh
            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += RefreshTable;
            TableView.Add(RefreshControl);

            // Set right button initially to edit 
            NavigationItem.LeftBarButtonItem = edit;
            NavigationItem.RightBarButtonItem = SettingsButton;
        }

        partial void SettingsButton_Activated(UIBarButtonItem sender)
        {
            if(Globals.LocalLogin)
            {
                UITableViewControllerLocalSettingsScreen uITableViewControllerLocalSettingsScreen =
                    this.Storyboard.InstantiateViewController(strings.LocalSettingsScreen)
                          as UITableViewControllerLocalSettingsScreen;
                if (uITableViewControllerLocalSettingsScreen != null)
                {
                    NavigationController.PushViewController(uITableViewControllerLocalSettingsScreen, true);
                }
            }
            else
            {
                UITableViewControllerGlobalSettingsScreen uITableViewControllerGlobalSettingsScreen =
                    this.Storyboard.InstantiateViewController(strings.GlobalSettingsScreen)
                         as UITableViewControllerGlobalSettingsScreen;
                if (uITableViewControllerGlobalSettingsScreen != null)
                {
                    NavigationController.PushViewController(uITableViewControllerGlobalSettingsScreen, true);
                }
            }
        }

        //Method Pull to refresh
        void RefreshTable(object sender, EventArgs e)
        {
            RefreshControl.BeginRefreshing();
            RefreshDeviceList();
            TableView.ReloadData();
            RefreshControl.EndRefreshing();
        }
    }
}
