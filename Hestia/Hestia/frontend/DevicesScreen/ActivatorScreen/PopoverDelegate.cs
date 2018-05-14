using System;
using UIKit;

namespace Hestia.DevicesScreen.ActivatorScreen
{
    public class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        public PopoverDelegate()
        {
        }

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
