using UIKit;
using Foundation;
using CoreGraphics;

namespace Hestia.DevicesScreen.AddDeviceScreen
{
    /// <summary>
    /// This view controls how the cells on the addDeviceProperties are displayed.
    /// <see cref="TableSourceAddDeviceProperties"/>
    /// </summary>
    public class PropertyCell : UITableViewCell
    {
        public UITextField inputField;
        const int IndentationLeft = 15;
        const int IndentationTop = 4;

        public PropertyCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            // Create inputfield and add it to the cell
            inputField = new UITextField();
            ContentView.AddSubviews(new UIView[] {inputField});
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            inputField.Frame = new CGRect(IndentationLeft, IndentationTop, ContentView.Bounds.Width - 2 * IndentationLeft, ContentView.Bounds.Height - 2 * IndentationTop);
        }
    }
}
