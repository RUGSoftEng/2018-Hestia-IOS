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
    public partial class UITableViewControllerDevicesMain : UITableViewController, ISpeech
    {
        const int TableViewHeaderHeight = 35;
        const int TableViewHeaderTopPadding = 5;
        const int IconDimension = 50;

        private SpeechRecognition speechRecognizer;

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

            button.TouchDown += (object sender, EventArgs e) =>
            {
                if(!isEditing)
                {
                    speechRecognizer = new SpeechRecognition(this);
                    speechRecognizer.StartRecording();
                }
            };

            button.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (isEditing)
                { // segue to add device
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

        private void ProcessSpeechResult(string result)
        {
            string resultLower = result.ToLower();

            if (resultLower.Equals("local"))
            {

            }
            else if (resultLower.Equals("global"))
            {

            }
            else if (resultLower == null)
            {
                new WarningMessage("Something went wrong", "Please make sure you have allowed speech recognition and try again.", this);
            }
            else
            {
                new WarningMessage("Speech could not be recognized", "Please try again.", this);
            }
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

        public void ProcessSpeech(string result)
        {
            Device device;
            result = result.ToLower();
            if (result.Contains(value: "activate") ||
                (result.Contains(value: "turn") && result.Contains(value: "on")))
            {
                device = GetDevice(result);
                if (device != null)
                {
                    SetDevice(device, true);
                }
            }
            else if (result.Contains(value: "deactivate") ||
              (result.Contains(value: "turn") && result.Contains(value: "off")))
            {
                device = GetDevice(result);
                if (device != null)
                {
                    SetDevice(device, false);
                }
                else
                {
                    new WarningMessage("No device found", "Please make sure you have pronounce the device name correctly", this);
                }
            } 
            else if( result.Contains(value: "add device") ||
                      (result.Contains(value: "new device")))
            {
                ((TableSourceDevicesMain)DevicesTable.Source).InsertAction();
            } 
            else if (result.Contains(value: "edit")) 
            {
                device = GetDevice(result);
                if (device != null)
                {
                    UIViewControllerEditDeviceName editViewController = new UIViewControllerEditDeviceName(this);
                    editViewController.device = device;
                    this.NavigationController.PushViewController(editViewController, true);
                }
                else 
                {    
                    new WarningMessage("No device found", "Please make sure you have pronounce the device name correctly", this);
                }
            } 
            else if (result == null) 
            {
                new WarningMessage("Something went wrong", "Please make sure you have allowed speech recognition and try again.", this);
            }
            else
            {
                new WarningMessage(result + " is not a command.", "Please try again.", this);
            }
        }

        public void SetDevice(Device device, bool on_off)
        {

            foreach (Hestia.backend.models.Activator act in device.Activators)
            {
                if (act.State.Type == "bool")
                {
                    try
                    {
                        act.State = new ActivatorState(rawState: on_off, type: "bool");
                        RefreshControl.BeginRefreshing();
                        RefreshDeviceList();
                        TableView.ReloadData();
                        RefreshControl.EndRefreshing();
                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while changing activator state");
                        Console.WriteLine(ex.ToString());
                    }
                    return;
                }
            }
        }

        public Device GetDevice(string result)
        {
            List<Device> list = Globals.GetDevices();
            foreach (Device device in list)
            {
                if (result.Contains(value: device.Name.ToLower()))
                {
                    return device;
                }
            }
            return null;
        }
    }
}
