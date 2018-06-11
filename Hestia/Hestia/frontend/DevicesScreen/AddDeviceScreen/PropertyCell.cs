using UIKit;
using Foundation;
using CoreGraphics;

namespace Hestia.DevicesScreen.AddDeviceScreen
{
    /// <summary>
    /// This is the cell that is used in the Add devices screen, where one has to enter the information
    /// for a new device. It contains an input textfield.
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

        /// <summary>
        /// Determines the position of the input textfield in the cell.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            inputField.Frame = new CGRect(IndentationLeft, IndentationTop, ContentView.Bounds.Width - 2 * IndentationLeft, ContentView.Bounds.Height - 2 * IndentationTop);
        }
    }
}
