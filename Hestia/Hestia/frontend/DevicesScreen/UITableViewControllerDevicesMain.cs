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
using Foundation;

namespace Hestia.DevicesScreen
{
    /// <summary>
    /// This class contains the contents and behaviour of the ViewController that controls the Devices main screen.
    /// It sets the buttons in the navigation bar, manages the refreshing of the list and defines the behaviour of SpeechRecognition.
    /// </summary>
    public partial class UITableViewControllerDevicesMain : UITableViewController, IViewControllerSpeech
    {
        const int TableViewFooterHeight = 50;
        const int IconDimension = 50;
        const int Padding = 15;
        nfloat bottomOfView;

        SpeechRecognition speechRecognizer;
        enum Warning { AccessDenied = 1, RecordProblem };

       // Done button in top left (appears in edit mode)
        UIBarButtonItem done;
        // Edit button in top left (is shown initially)
        UIBarButtonItem edit;

        List<Device> devices;

        public UITableViewControllerDevicesMain(IntPtr handle) : base(handle)
        {
        }

        /// <summary>
        /// This methods is called if the done button is touched. The button is changed to display "edit" and the 
        /// list changes to normal mode (the delete icons disappear).
        /// See, <see cref="TableSourceDevicesMain.DidFinishTableEditing(UITableView)"/>
        /// </summary>
        public void CancelEditingState()
        {
            DevicesTable.SetEditing(false, true);
            NavigationItem.LeftBarButtonItem = edit;
            ((TableSourceDevicesMain)DevicesTable.Source).DidFinishTableEditing(DevicesTable);
        }

        /// <summary>
        /// This methods is called if the edit button is touched. The button is changed to display "done" and the 
        /// list changes to editing mode (the delete icons appear).
        /// See, <see cref="TableSourceDevicesMain.WillBeginTableEditing(UITableView))"/>
        /// </summary>
        public void SetEditingState()
        {
            ((TableSourceDevicesMain)DevicesTable.Source).WillBeginTableEditing(DevicesTable);
            DevicesTable.SetEditing(true, true);
            NavigationItem.LeftBarButtonItem = done;
        }

        /// <summary>
        /// This method should be called if the list of devices should be updated from the server. 
        /// This method is called for example when pull-to-refresh is performed.
        /// </summary>
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
            else // Global login (possibly multiple servers)
            {
                // Complete list of devices, used in SpeechRecognition
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
            Console.WriteLine("Exception while getting devices from server");
            Console.WriteLine(ex);
            WarningMessage.Display("Could not refresh devices", "Exception while getting devices from server", this);
            // Show an empty list
            source.serverDevices = new List<List<Device>>();
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
            button.Frame = new CGRect(TableView.Bounds.Width - IconDimension - Padding, bottomOfView - TableViewFooterHeight - Padding , IconDimension, IconDimension);
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
                if (!isEditing)
                {
                    speechRecognizer = new SpeechRecognition(this);
                    speechRecognizer.StartRecording(out int warningStatus);
                    if (warningStatus == (int)Warning.AccessDenied) // Access to speech recognition denied
                    {
                        WarningMessage.Display(strings.speechAccessDenied, strings.speechAllowAccess, this);
                    }
                    else if (warningStatus == (int)Warning.RecordProblem) // Couldn't start speech recording
                    {
                        WarningMessage.Display(strings.speechStartRecordProblem, strings.tryAgain, this);
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
                if (!isEditing)
                {
                    speechRecognizer.CancelRecording();
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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ReloadButtons(DevicesTable.Editing);
        }


        /// <summary>
        /// This method is called when the Devices main screen first appears and loads the header with the
        /// Speech Recognition icon, refreshes the devices list and sets the edit/done and settings buttons
        /// </summary>
        public override void ViewDidLoad()
        { 
            base.ViewDidLoad();

            SpeechRecognition.RequestAuthorization();

            // Fix the bottom position of the view, such that icons appear at same place when reloaded.
            bottomOfView = TableView.Bounds.Bottom;

            DevicesTable.TableFooterView = GetTableViewFooter();
            RefreshDeviceList();
            // Set the light gray color in Globals, it is used in the Edit device (change name) screen
            Globals.DefaultLightGray = TableView.BackgroundColor;
            // To be able to tap a row in editing mode for changing name
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

        /// <summary>
        /// This method makes sure the device list is refreshed if the Main devices screen reappears
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            RefreshDeviceList();
            TableView.ReloadData();
        }

        /// <summary>
        /// The settings button segues to the local or global settings screen depending on the used mode.
        /// </summary>
        partial void SettingsButton_Activated(UIBarButtonItem sender)
        {
            if(Globals.LocalLogin)
            {
                UITableViewControllerLocalSettingsScreen uITableViewControllerLocalSettingsScreen =
                    this.Storyboard.InstantiateViewController(strings.LocalSettingsScreen) as UITableViewControllerLocalSettingsScreen;
                if (uITableViewControllerLocalSettingsScreen != null)
                {
                    NavigationController.PushViewController(uITableViewControllerLocalSettingsScreen, true);
                }
            }
            else
            {
                UITableViewControllerGlobalSettingsScreen uITableViewControllerGlobalSettingsScreen =
                    this.Storyboard.InstantiateViewController(strings.GlobalSettingsScreen) as UITableViewControllerGlobalSettingsScreen;
                if (uITableViewControllerGlobalSettingsScreen != null)
                {
                    NavigationController.PushViewController(uITableViewControllerGlobalSettingsScreen, true);
                }
            }
            sender.Enabled = false;
        }

        /// <summary>
        /// Method for Pull to refresh
        /// </summary>
        void RefreshTable(object sender, EventArgs e)
        {
            RefreshControl.BeginRefreshing();
            RefreshDeviceList();
            TableView.ReloadData();
            RefreshControl.EndRefreshing();
        }

        /// <summary>
        /// This method processes the speech recognition result.
        /// It can perform five actions: turning on a device, turning off a device, adding a new device, editing a device and removing a device.
        /// </summary>
        /// <param name="result"></param>
        public void ProcessSpeech(string result)
        {
            Device device;
            if (result == null)
            {   // Something went wrong with recognizing speech
                WarningMessage.Display(strings.speechError, strings.tryAgain, this);
            }
            else
            {
                result = result.ToLower();
                if (result.Contains("activate") || (result.Contains("turn") && result.Contains("on")))
                {   // Turning on a device
                    device = GetDevice(result);
                    if (device != null)
                    {
                        SetDevice(device, true);
                    }
                }
                else if (result.Contains("deactivate") || (result.Contains("turn") && result.Contains("off")))
                {   // Turning off a device
                    device = GetDevice(result);
                    if (device != null)
                    {
                        SetDevice(device, false);
                    }
                    else
                    {
                        WarningMessage.Display(strings.noDeviceFound, strings.pronounceDeviceNameCorrectly, this);
                    }
                }
                else if (result.Contains("add device") || (result.Contains("new device")))
                {   // Adding a new device
                    ((TableSourceDevicesMain)DevicesTable.Source).InsertAction();
                }
                else if (result.Contains("edit"))
                {   // Editing a device
                    device = GetDevice(result);
                    if (device != null)
                    {
                        UIViewControllerEditDeviceName editViewController = new UIViewControllerEditDeviceName(this);
                        editViewController.device = device;
                        NavigationController.PushViewController(editViewController, true);
                    }
                    else
                    {
                        WarningMessage.Display(strings.noDeviceFound, strings.pronounceDeviceNameCorrectly, this);
                    }
                }
                else if (result.Contains("remove") || result.Contains("delete"))
                {   // Removing a device
                    device = GetDevice(result);
                    if (device != null)
                    {
                        // Loop over devices until device is found
                        for (int section = 0; section < ((TableSourceDevicesMain)DevicesTable.Source).serverDevices.Count; section++)
                        {
                            var devices = ((TableSourceDevicesMain)DevicesTable.Source).serverDevices[section];
                            for (int row = 0; row < devices.Count; row++)
                            {
                                if (device.DeviceId.Equals(devices[row].DeviceId))
                                {
                                    if (Globals.LocalLogin)
                                    {
                                        try
                                        {   // Remove device from server   
                                            Globals.LocalServerinteractor.RemoveDevice(devices[row]);
                                        }
                                        catch (ServerInteractionException ex)
                                        {
                                            Console.WriteLine("Exception while removing device. (Bug in server: exception is always thrown)");
                                            Console.Out.WriteLine(ex);
                                        }
                                    }
                                    else // Global login
                                    {
                                        var deviceServerInteractor = devices[row].ServerInteractor;
                                        try
                                        {
                                            deviceServerInteractor.RemoveDevice(devices[row]);
                                        }
                                        catch (ServerInteractionException ex)
                                        {
                                            Console.WriteLine("Exception while removing device. (Bug in server: exception is always thrown)");
                                            Console.Out.WriteLine(ex);
                                        }
                                    }

                                    // Remove device from list with devices and refresh device list
                                    ((TableSourceDevicesMain)DevicesTable.Source).serverDevices[section].RemoveAt(row);
                                    RefreshDeviceList();
                                    TableView.ReloadData();
                                }
                            }
                        }
                    }
                    else
                    {
                        WarningMessage.Display(strings.noDeviceFound, strings.pronounceDeviceNameCorrectly, this);
                    }
                }
                else
                {   // Result did not contain any of the above keywords
                    WarningMessage.Display(result + " " + strings.speechNotACommand, strings.tryAgain, this);
                }
            }
        }

        /// <summary>
        /// Sets the boolean state of a device.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="on_off"></param>
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

        /// <summary>
        /// Searches for a device given a string which may or may not contain the device name.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>A device object</returns>
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
