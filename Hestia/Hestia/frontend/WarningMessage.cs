using UIKit;

namespace Hestia.frontend
{
    /// <summary>
    /// This class contains code to simply create the typical iOS Warning message in one line of code.
    /// It is used in many places in the other front end classes.
    /// </summary>
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

        /// <summary>
        /// Displays a warning message
        /// </summary>
        /// <param name="title">Title of the message</param>
        /// <param name="message">Message body</param>
        /// <param name="uIViewController">The ViewController on which the warning should be presented</param>
        public static void Display(string title, string message, UIViewController uIViewController)
        {
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            uIViewController.PresentViewController(okAlertController, true, null);
        }
    }
}
