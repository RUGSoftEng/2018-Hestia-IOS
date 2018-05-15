using UIKit;
using Foundation;
using CoreGraphics;


namespace Hestia.DevicesScreen.AddDeviceScreen
{
    public class PropertyCell : UITableViewCell
    {
        public UITextField inputField;
        public PropertyCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            // Create inputfield and add it to the cell
            inputField = new UITextField();
            ContentView.AddSubviews(new UIView[] {inputField});
        }

        // Sets the placeholder from the required info
        public void UpdateCell(string placeholder)
        {
            inputField.Placeholder = "";
        }

        public override void LayoutSubviews()
        {
           base.LayoutSubviews();
           inputField.Frame = new CGRect(15, 4, ContentView.Bounds.Width - 30, ContentView.Bounds.Height - 8);
        }
    }
}
