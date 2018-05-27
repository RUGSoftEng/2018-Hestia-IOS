using System;
using UIKit;
using Hestia.backend.models;

namespace Hestia.DevicesScreen.ActivatorScreen
{
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
