using System;
using UIKit;
using Hestia.Backend.Models;

namespace Hestia.Frontend.DevicesScreen.ActivatorScreen
{
    /// <summary>
    /// The TableViewController in which the TableView containing the list with 
    /// activators and their names. This is presented as a pop-over in the Devices main screen.
    /// </summary>
    public partial class UITableViewActivators : UITableViewController
    {
        public Device device;
        public Device Device
        {
            get => device;
            set => device = value;
        }

        public UITableViewActivators (IntPtr handle) : base (handle)
        {
        }

        public UITableViewActivators()
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();

            TableView.AllowsSelection = false;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceActivators(device, this);
		}
	}
}
