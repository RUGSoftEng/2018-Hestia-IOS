using System;
using System.Collections;
using UIKit;
using Hestia.backend.models;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.AddDeviceScreen;
using Hestia.DevicesScreen.resources;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceProperties : UITableViewController
    {
        // The table that lives in this view controller
        UITableView table;
        // Plugin info set at creation in devices window
        public PluginInfo pluginInfo;
        // Keeps track at the input fields for device properties
        public Hashtable inputFields = new Hashtable();
       

        public UITableViewControllerAddDeviceProperties(IntPtr handle) : base(handle)
        {
        }

        // Called on press of the save button. It copies the values that are input 
        // in the text fields to the pluginInfo opbject
        public void saveFields()
        {
            // Used to loop through original property names
            string[] propertyNames = new string[pluginInfo.RequiredInfo.Keys.Count];
            pluginInfo.RequiredInfo.Keys.CopyTo(propertyNames, 0);
            foreach (string property in propertyNames)
            {
                pluginInfo.RequiredInfo[property] = ((PropertyCell)inputFields[property]).inputField.Text;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style

            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceProperties(this);

            // Add the table to the view
            Add(table);

            // Save button
            UIBarButtonItem save = new UIBarButtonItem(UIBarButtonSystemItem.Save, (sender, eventArguments) => {
                this.saveFields();
                try
                {
                    Globals.ServerInteractor.AddDevice(pluginInfo);
                    // UIApplication.SharedApplication.KeyWindow.RootViewController;
                    // Console.WriteLine(tt);
                    // rootvc.table.SetEditing(false, false);
                    //this.NavigationController.SetEditing(false, false);
                    //((UITableView)rootvc.View).SetEditing(false, false);

                    var rootvc = this.NavigationController.ViewControllers[0] as UITableViewControllerDevicesMain;
                    rootvc.setEdit();
                    this.NavigationController.PopToViewController(rootvc, true);
                }
                catch (Exception except)
                {
                    Console.WriteLine("Exception while adding device");
                    Console.WriteLine(except.StackTrace);
                }
               
            });

            // Set right button to save 
            NavigationItem.RightBarButtonItem = save;
        }

      

    }
}