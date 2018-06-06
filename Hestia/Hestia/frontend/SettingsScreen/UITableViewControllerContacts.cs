using System;
using UIKit;
using MessageUI;

namespace Hestia
{
    public partial class UITableViewControllerContacts : UITableViewController
    {
        MFMailComposeViewController mailController;

        public UITableViewControllerContacts(IntPtr handle) : base(handle)
        {
        }

        //Opinion
        partial void UIButton105005_TouchUpInside(UIButton sender)
        {
            const string subject = "Opinion on Hestia iOS Application";
            const string message = "Test";
            ComposeMessage(subject, message);
        }

        //Bugs
        partial void UIButton105006_TouchUpInside(UIButton sender)
        {
            const string subject = "Bug(s) found in Hestia iOS Application";
            const string message = "Test";
            ComposeMessage(subject, message);
        }

        //Issues
        partial void UIButton105007_TouchUpInside(UIButton sender)
        {
            const string subject = "Issues Hestia iOS Application";
            const string message = "Test";
            ComposeMessage(subject, message);
        }

        void ComposeMessage(string subject, string message)
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new string[] { "hestia.contact.email@gmail.com" });
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
