using UIKit;

namespace Hestia.frontend
{
    public class WarningMessage
    {
        public WarningMessage(string title, string message, UIViewController uIViewController)
        {
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            uIViewController.PresentViewController(okAlertController, true, null);
        }
    }
}
