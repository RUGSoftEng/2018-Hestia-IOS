using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.AddDeviceScreen;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceProperties : UITableViewController
    {
        UITableView table;

        // The table that lives in this view controller

        // Plugin info set at creation in devices window
        public PluginInfo pluginInfo;
        public Hashtable inputFields = new Hashtable();
        string[] propertyNames;

        public UITableViewControllerAddDeviceProperties(IntPtr handle) : base(handle)
        {
           
        }

        public void saveFields()
        {
            propertyNames = new string[pluginInfo.RequiredInfo.Keys.Count];
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
            UIBarButtonItem save = new UIBarButtonItem(UIBarButtonSystemItem.Save, (s, e) => {
                this.saveFields();
                try
                {
                    Globals.ServerInteractor.AddDevice(pluginInfo);
                }
                catch (Exception except)
                {
                    Console.WriteLine("Exception while adding device");
                    Console.WriteLine(except.StackTrace);
                }
               
            });

            // Set right button initially to edit 
            NavigationItem.RightBarButtonItem = save;
        }

      

    }
}