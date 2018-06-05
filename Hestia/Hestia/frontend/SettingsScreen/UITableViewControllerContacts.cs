using Foundation;
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
            if(MFMailComposeViewController.CanSendMail){
                mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new string[] {"hestia.contact.email@gmail.com"});
                mailController.SetSubject("Opinion on Hestia iOS Application");
                mailController.SetMessageBody("This is a test", false);

                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };
                this.PresentViewController(mailController, true, null);
            }
        }

        //Bugs
        partial void UIButton105006_TouchUpInside(UIButton sender)
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new string[] {"hestia.contact.email@gmail.com"});
                mailController.SetSubject("Bug(s) found in Hestia iOS Application");
                mailController.SetMessageBody("This is a test", false);

                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };
                this.PresentViewController(mailController, true, null);
            }
        }

        //Issues
        partial void UIButton105007_TouchUpInside(UIButton sender)
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new string[] {"hestia.contact.email@gmail.com"});
                mailController.SetSubject("Issues of Hestia iOS Application");
                mailController.SetMessageBody("This is a test", false);

                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };
                this.PresentViewController(mailController, true, null);
            }
        }
    
    }
}