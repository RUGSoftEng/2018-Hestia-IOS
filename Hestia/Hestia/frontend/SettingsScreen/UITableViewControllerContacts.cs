using System;
using UIKit;
using MessageUI;
using Hestia.Resources;

namespace Hestia.Frontend.SettingsScreen
{
    /// <summary>
    /// This class contains the behaviour of the buttons int the contacts page of the settings screen.
    /// That screen contains three buttons that launch an email client to send a message to the
    /// Hestia developers with different subjects.
    /// </summary>
    public partial class UITableViewControllerContacts : UITableViewController
    {
        MFMailComposeViewController mailController;

        public UITableViewControllerContacts(IntPtr handle) : base(handle)
        {
        }

        /// <summary>
        /// Button that sends a message to give opion on the Hestia iOS App
        /// </summary>
        partial void UIButton105005_TouchUpInside(UIButton sender)
        {
            const string Subject = "Opinion on Hestia iOS Application";
            const string Message = "Test";
            ComposeMessage(Subject, Message);
        }

        /// <summary>
        /// Button that sends a message to report bugs in the Hestia iOS App
        /// </summary>
        partial void UIButton105006_TouchUpInside(UIButton sender)
        {
            const string Subject = "Bug(s) found in Hestia iOS Application";
            const string Message = "Test";
            ComposeMessage(Subject, Message);
        }

        /// <summary>
        /// Button that sends a message to report issues regarding the Hestia iOS App
        /// </summary>
        partial void UIButton105007_TouchUpInside(UIButton sender)
        {
            const string Subject = "Issues Hestia iOS Application";
            const string Message = "Test";
            ComposeMessage(Subject, Message);
        }

        /// <summary>
        /// This methods creates a new email message with the given subject and message body
        /// </summary>
        /// <param name="subject">Subject of the message</param>
        /// <param name="message">The body of the message</param>
        void ComposeMessage(string subject, string message)
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new string[] { strings.hestiaEmail });
                mailController.SetSubject(subject);
                mailController.SetMessageBody(message, false);

                mailController.Finished += (object s, MFComposeResultEventArgs args) =>
                {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };
                PresentViewController(mailController, true, null);
            }
        }
    }
}
