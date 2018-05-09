using Foundation;
using UIKit;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using Hestia.backend.utils;
using Hestia.backend;
using System;
using Hestia.Resources;

namespace Hestia
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public static UIStoryboard mainStoryboard = UIStoryboard.FromName("Main", null);
        public static UIStoryboard devices2Storyboard = UIStoryboard.FromName("Devices2", null);

        string defaultServername;
        string defaultIP;
        string defaultPort;

        public bool IsServerValid()
        {
            try
            {
                bool validIp = PingServer.Check(defaultIP, int.Parse(defaultPort));
            }
            catch (Exception exception)
            {
                Console.Write(exception.StackTrace);
                return false;
            }
            return true;
        }

        public void SetGlobalsToDefaults()
        {
            Globals.ServerName = defaultServername;
            Globals.IP = defaultIP;

            Globals.Port = int.Parse(defaultPort);
            ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
            Globals.LocalServerinteractor = serverInteractor;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

            // For Debugging
           // Globals.ResetUserDefaults();
           // userDefaults.RemoveObject(strings.defaultsServerNameHestia);
           // userDefaults.RemoveObject(strings.defaultsIpHestia);
           // userDefaults.RemoveObject(strings.defaultsPortHestia);
           // userDefaults.RemoveObject(strings.defaultsLocalHestia);

            string defaultLocal = userDefaults.StringForKey(Resources.strings.defaultsLocalHestia);
            defaultIP = userDefaults.StringForKey(Resources.strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(Resources.strings.defaultsPortHestia);
            defaultServername = userDefaults.StringForKey(Resources.strings.defaultsServerNameHestia);

            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            // No previous login information available. Go to local/global choose screen.
            if (defaultLocal == null)
            {
                UIViewControllerLocalGlobal localGlobalViewController = mainStoryboard.InstantiateInitialViewController() as UIViewControllerLocalGlobal;
                Window.RootViewController = localGlobalViewController;
                Window.MakeKeyAndVisible();
            }
            else if(defaultLocal == bool.TrueString)
            {
                Globals.LocalLogin = true;
                UITableViewControllerServerConnect serverConnectViewController = devices2Storyboard.InstantiateInitialViewController() as UITableViewControllerServerConnect;
                //Window.RootViewController = serverConnectViewController;

                if(IsServerValid())
                {
                    UINavigationController navigationController = devices2Storyboard.InstantiateViewController("navigationDevicesMain")
                                    as UINavigationController;
                    Window.RootViewController = navigationController;
                    SetGlobalsToDefaults();
                }
                else
                {
                    Window.RootViewController = serverConnectViewController;
                }
                Window.MakeKeyAndVisible();
            }
            else
            {
                Globals.LocalLogin = false;
                UIViewControllerMain auth0ViewController = mainStoryboard.InstantiateViewController("auth0ViewController") as UIViewControllerMain;
                Window.RootViewController = auth0ViewController;
                //TODO Make a method in serverconnect that check login, if so perform already next segue
                Window.MakeKeyAndVisible();
            }

            return true;
        }


        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        { 
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}