using Foundation;
using UIKit;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using Hestia.backend.utils;
using Hestia.backend;
using Auth0.OidcClient;
using System;
using Hestia.Resources;
using Hestia.backend.exceptions;

namespace Hestia
{
    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }
        public static UIStoryboard mainStoryboard = UIStoryboard.FromName(strings.mainStoryBoard, null);
        public static UIStoryboard devices2Storyboard = UIStoryboard.FromName(strings.devices2StoryBoard, null);

        string defaultServername;
        string defaultIP;
        string defaultPort;
        string defaultAuth0AccessToken;

        public bool IsServerValid()
        {
            if (defaultIP == null)
            {
                return false;
            }
            try
            {
                bool validIp = PingServer.Check(defaultIP, int.Parse(defaultPort));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
            return true;
        }

        public void SetGlobalsToDefaultsLocalLogin()
        {
            Globals.ServerName = defaultServername;
            Globals.Address = defaultIP;
            HestiaServerInteractor serverInteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address, int.Parse(strings.defaultPort)));
            Globals.LocalServerinteractor = serverInteractor;
        }

        public void SetGlobalsToDefaultsGlobalLogin()
        {
            HestiaWebServerInteractor hestiaWebServerInteractor = new HestiaWebServerInteractor(new NetworkHandler(strings.webserverIP, defaultAuth0AccessToken));

            try
            {
                hestiaWebServerInteractor.PostUser(); 
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while posting user. User possibly already exists.");
                Console.WriteLine(ex.StackTrace);
            }
            Globals.Auth0Servers = new System.Collections.Generic.List<backend.models.HestiaServer>();
            try
            {
                Globals.Auth0Servers = hestiaWebServerInteractor.GetServers();
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting devices from server.");
                Console.WriteLine(ex.StackTrace);
            }
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Xamarin.Calabash.Start();
            Globals.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            Globals.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

            string defaultLocal = userDefaults.StringForKey(strings.defaultsLocalHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(strings.defaultsPortHestia);
            defaultServername = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultAuth0AccessToken = userDefaults.StringForKey(strings.defaultsAccessTokenHestia);
            Console.WriteLine(" Defaultip:" + defaultIP);

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            Console.WriteLine(" defaults:" + defaultLocal);
            // Check if defaults for local/global are present
            if (defaultLocal == bool.TrueString)
            {
				Console.WriteLine(" Default local ");
                Globals.LocalLogin = true;
                var serverConnectViewController = devices2Storyboard.InstantiateInitialViewController();

                if(IsServerValid())
                {
                    UINavigationController navigationController = devices2Storyboard.InstantiateViewController(strings.navigationControllerDevicesMain)
                        as UINavigationController;
                    Window.RootViewController = navigationController;
                    SetGlobalsToDefaultsLocalLogin();
                }
                else
                {
                    Window.RootViewController = serverConnectViewController;
                }
            }

            else if (defaultLocal == bool.FalseString)
            {
				Console.WriteLine(" Default global");
                Globals.LocalLogin = false;

                var viewServerList = devices2Storyboard.InstantiateViewController("navigationServerList");
                Window.RootViewController = viewServerList;
                SetGlobalsToDefaultsGlobalLogin();
            }
            else
            {   // No previous login information available. Go to local/global choose screen.
                UIViewControllerLocalGlobal localGlobalViewController = mainStoryboard.InstantiateInitialViewController() as UIViewControllerLocalGlobal;
                Window.RootViewController = localGlobalViewController;
            }
            Window.MakeKeyAndVisible();
            return true;
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            ActivityMediator.Instance.Send(url.AbsoluteString);
            return true;
        }
    }
}
