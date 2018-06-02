using UIKit;

namespace Hestia.frontend
{
    public class WarningMessage
    {
        string title, message;
        UIAlertController alertController;

        public WarningMessage(string title, string message, UIViewController uIViewController)
        {
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            uIViewController.PresentViewController(okAlertController, true, null);
        }

        public WarningMessage(string title, string message)
        {
            this.title = title;
            this.message = message;
            alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
        }

        public void DisplayWarningMessage(UIViewController uIViewController)
        {
            uIViewController.PresentViewController(alertController, true, null);
        }
    }
}
