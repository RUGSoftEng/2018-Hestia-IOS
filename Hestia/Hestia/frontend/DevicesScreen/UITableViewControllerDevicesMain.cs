using System;
using UIKit;

using System.Collections.Generic;

using Hestia.DevicesScreen.resources;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.backend;
using CoreGraphics;

namespace Hestia.DevicesScreen
{
    public partial class UITableViewControllerDevicesMain : UITableViewController
    {
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
            ((TableSource)DevicesTable.Source).DidFinishTableEditing(DevicesTable);
        }

        public void SetEditingState()
        {
            ((TableSource)DevicesTable.Source).WillBeginTableEditing(DevicesTable);
            DevicesTable.SetEditing(true, true);
            NavigationItem.LeftBarButtonItem = done;
        }

        public void RefreshDeviceList()
        {
            TableSource source = new TableSource(this);
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
                        Console.WriteLine("Exception while getting devices from local server " + interactor.NetworkHandler.Ip + interactor.IsRemoteServer);
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            DevicesTable.Source = source;
        }

        public UIView HeaderV(bool isEditing)
        {

            //  UITableViewCell cell = new UITableViewCell(UITableViewCellStyle.Default, "test");
            //cell.BackgroundColor = UIColor.White;
            //cell.TextLabel.Text = state;
            //cell.Editing = true;

            ////return cell;
            UIView view = new UIView(new CGRect(0, 0, TableView.Bounds.Width, 45));




           // new CGRect()
            //view.AddSubview(titleLabel);
            var imageview = new UIImageView(new CGRect(TableView.Bounds.Width/2 - 25, 5, 50, 50));

            if (isEditing)
            {
                imageview.Image = UIImage.FromBundle("AddDeviceIcon");
            }
            else
            {
                imageview.Image = UIImage.FromBundle("VoiceControlIcon");
            }

            //var buttonFrame = new CGRect(view.Frame.Width - 70, 0, 60, 22);

            UIButton button = new UIButton(UIButtonType.System);
            button.Frame = new CGRect(TableView.Bounds.Width / 2 - 25, 5, 50, 50);
           // button.Layer.CornerRadius = 5;
            //button.BackgroundColor = UIColor.Black;
            //button.SetTitleColor(UIColor.White, UIControlState.Normal);
            if (isEditing)
            {
               
                button.SetBackgroundImage(UIImage.FromBundle("AddDeviceIcon"), UIControlState.Normal);
            }
            else
            {
                button.SetBackgroundImage(UIImage.FromBundle("VoiceControlIcon"), UIControlState.Normal);

            }


            //  button.SetTitle("StackOverflow", UIControlState.Normal);
            //  button.TitleEdgeInsets = new UIEdgeInsets(0, -button.ImageView.Frame.Size.Width, 0, button.ImageView.Frame.Size.Width);
              button.ImageEdgeInsets = new UIEdgeInsets(0,0, 0, 0);

            //// I had to create subclass of UIbutton, normal new UIButton() was not working
            //var editButton = new CustomEditButton(buttonFrame2);

            //editButton.TouchUpInside += delegate
            //{
            //    if (EditButtonClicked != null)
            //    {
            //        EditButtonClicked(sectionType, editButton);
            //    }
            //};

            // view.AddSubview(imageview);
            view.AddSubview(button);
            return view;
        
        }

		public override void ViewDidLoad()
        { 
            base.ViewDidLoad();

            DevicesTable.TableHeaderView = HeaderV(false);
    
            //DevicesTable.TableHeaderView.
            RefreshDeviceList();
            Globals.DefaultLightGray = TableView.BackgroundColor;
            // To tap row in editing mode for changing name
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
                     this.Storyboard.InstantiateViewController("LocalSettingsScreen")
                          as UITableViewControllerLocalSettingsScreen;
                if (uITableViewControllerLocalSettingsScreen != null)
                {
                    NavigationController.PushViewController(uITableViewControllerLocalSettingsScreen, true);
                }
            }
            else
            {
                UITableViewControllerGlobalSettingsScreen uITableViewControllerGlobalSettingsScreen =
                    this.Storyboard.InstantiateViewController("GlobalSettingsScreen")
                         as UITableViewControllerGlobalSettingsScreen;
                if (uITableViewControllerGlobalSettingsScreen != null)
                {
                    NavigationController.PushViewController(uITableViewControllerGlobalSettingsScreen, true);
                }
            }
        }

        //Method Pull to refresh
        private void RefreshTable(object sender, EventArgs e)
        {
            RefreshControl.BeginRefreshing();
            RefreshDeviceList();
            TableView.ReloadData();
            RefreshControl.EndRefreshing();

        }
    }
}
