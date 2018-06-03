using CoreGraphics;
using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.backend;
using Hestia.frontend;
using Hestia.Resources;
using UIKit;
using System;
using System.Collections.Generic;
using Hestia.backend.speech_recognition;
using Hestia.DevicesScreen.EditDevice;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerDevicesMain : UITableViewController, IViewControllerSpeech
    {
        const int TableViewHeaderHeight = 35;
        const int TableViewHeaderTopPadding = 5;
        const int IconDimension = 50;

        SpeechRecognition speechRecognizer;

       // Done button in top right (appears in edit mode)
        UIBarButtonItem done;
        // Edit button in top right (is shown initially)
        UIBarButtonItem edit;

        List<Device> devices;

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
                source.numberOfServers = int.Parse(strings.defaultNumberOfServers);
                try
                {
                    devices = Globals.LocalServerinteractor.GetDevices();
                    source.serverDevices.Add(devices);
                }
                catch (ServerInteractionException ex)
                {
                    HandleException(source, ex);
                }
            }
            else
            {
                devices = new List<Device>();
                source.numberOfServers = Globals.GetNumberOfSelectedServers();
                foreach (HestiaServerInteractor interactor in Globals.GetInteractorsOfSelectedServers())
                {
                    try
                    {
                        List<Device> tempDevices = interactor.GetDevices();
                        source.serverDevices.Add(tempDevices);
                        devices.AddRange(tempDevices);
                    }
                    catch (ServerInteractionException ex)
                    {
                        HandleException(source, ex);
                    }
                }
            }
            DevicesTable.Source = source;
        }

        void HandleException(TableSourceDevicesMain source, ServerInteractionException ex)
        {
            Console.WriteLine("Exception while getting devices from local server");
            Console.WriteLine(ex);
            new WarningMessage("Could not refresh devices", "Exception while getting devices from local server", this);
            source.serverDevices = new List<List<Device>>();
            TableView.ReloadData();
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

            button.TouchDown += (object sender, EventArgs e) =>
            {
                if(!isEditing)
                {
                    speechRecognizer = new SpeechRecognition(this, this);
                    WarningMessage warningMessage = speechRecognizer.StartRecording();
                    if (warningMessage != null)
                    {
                        warningMessage.DisplayWarningMessage(this);
                    }
                }
            };

            button.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (isEditing)
                {   // segue to add device
                    ((TableSourceDevicesMain)DevicesTable.Source).InsertAction();
                }
                else
                {
                    speechRecognizer.StopRecording();
                }
            };

            button.TouchDragExit += (object sender, EventArgs e) =>
            {
                if(!isEditing)
                {
                    speechRecognizer.CancelRecording();
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

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            RefreshDeviceList();
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

        public void ProcessSpeech(string result)
        {
            Device device;
            result = result.ToLower();
            if (result.Contains("activate") || (result.Contains("turn") && result.Contains("on")))
            {
                device = GetDevice(result);
                if (device != null)
                {
                    SetDevice(device, true);
                }
            }
            else if (result.Contains("deactivate") || (result.Contains("turn") && result.Contains("off")))
            {
                device = GetDevice(result);
                if (device != null)
                {
                    SetDevice(device, false);
                }
                else
                {
                    new WarningMessage(strings.noDeviceFound, strings.pronounceDeviceNameCorrectly, this);
                }
            } 
            else if( result.Contains("add device") || (result.Contains("new device")))
            {
                ((TableSourceDevicesMain)DevicesTable.Source).InsertAction();
            } 
            else if (result.Contains("edit")) 
            {
                device = GetDevice(result);
                if (device != null)
                {
                    UIViewControllerEditDeviceName editViewController = new UIViewControllerEditDeviceName(this);
                    editViewController.device = device;
                    NavigationController.PushViewController(editViewController, true);
                }
                else 
                {
                    new WarningMessage(strings.noDeviceFound, strings.pronounceDeviceNameCorrectly, this);
                }
            }
            else
            {
                new WarningMessage(result + " " + strings.speechNotACommand, strings.tryAgain, this);
            }
        }

        public void SetDevice(Device device, bool on_off)
        {
            foreach (backend.models.Activator act in device.Activators)
            {
                if (act.State.Type == "bool")
                {
                    try
                    {
                        act.State = new ActivatorState(on_off, "bool");
                        RefreshControl.BeginRefreshing();
                        RefreshDeviceList();
                        TableView.ReloadData();
                        RefreshControl.EndRefreshing();
                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while changing activator state");
                        Console.WriteLine(ex);
                    }
                    return;
                }
            }
        }

        public Device GetDevice(string result)
        {
            foreach (Device device in devices)
            {
                if (result.Contains(device.Name.ToLower()))
                {
                    return device;
                }
            }
            return null;
        }
    }
}
