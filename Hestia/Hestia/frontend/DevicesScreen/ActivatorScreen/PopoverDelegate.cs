using UIKit;

namespace Hestia.DevicesScreen.ActivatorScreen
{
    /// <summary>
    /// This class defines the behaviour of the pop-over that is presented on the Devices main screen with the activators.
    /// </summary>
    public class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController)
        {
            return UIModalPresentationStyle.None;
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController controller, UITraitCollection traitCollection)
        {
            return UIModalPresentationStyle.None;
        }
    }
}
