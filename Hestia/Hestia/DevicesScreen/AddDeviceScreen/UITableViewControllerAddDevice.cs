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


namespace Hestia
{
    public partial class UITableViewControllerAddDevice : UITableViewController
    {
        UITableView table;

        List<string> collections;
        Hashtable collection_plugins;
        public UITableViewControllerAddDevice (IntPtr handle) : base (handle)
        {
            collection_plugins = new Hashtable();
        }


        // The table that lives in this view controller
       

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style
            //collections = new List<string>();
            Console.WriteLine(" Collections test");
            // The list with manufacturers
            try
            {
                collections = Globals.ServerInteractor.GetCollections();
                Console.WriteLine(" Collections test");
                Console.WriteLine(collections);
                foreach(string col in collections)
                {
                    Console.WriteLine(col);
                    List<string> plugins = Globals.ServerInteractor.GetPlugins(col);
                    Console.WriteLine(plugins[0]);
                    Console.WriteLine(plugins[1]);
                    collection_plugins.Add(col, plugins);
                }
     
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while using serverInteractor");
                Console.WriteLine(e.StackTrace);
            }
            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceManufacturer(collections, collection_plugins, this);

            // Add the table to the view
            Add(table);

        }
    }
}