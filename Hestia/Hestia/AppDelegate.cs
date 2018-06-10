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
using Hestia.backend.speech_recognition;
using Hestia.frontend;

namespace Hestia
{
    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    /// User Interface of the application, as well as listening (and optionally responding) to 
    /// application events from iOS. See the <see cref="FinishedLaunching(UIApplication, NSDictionary)"/> method, 
    /// that defines what happens if the app is started up.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }
        public static UIStoryboard mainStoryboard = UIStoryboard.FromName(strings.mainStoryBoard, null);
        public static UIStoryboard devices2Storyboard = UIStoryboard.FromName(strings.devices2StoryBoard, null);

        // To store the user defaults that are retrieved from the iPhone's memory.
        string defaultServername;
        string defaultIP;
        string defaultPort;
        string defaultAuth0AccessToken;

        /// <summary>
        /// This method checks if there is a valid IP for a local server saved in memory. It uses the <see cref="PingServer"/> class to 
        /// verify if a stored IP address is still valid.
        /// </summary>
        /// <returns><c>True</c>, if the server could be verified, <c>False</c> otherwise.</returns>
        public static bool IsServerValid(string defaultIP)
        {
            if (defaultIP == null)
            {
                return false;
            }
            try
            {
                string address = strings.defaultPrefix + defaultIP + ":" + int.Parse(strings.defaultPort);
                bool validIp = PingServer.Check(address);
            }
            catch (Exception ex)  // Not only catch ServerException, but also Exceptions that could be thrown following an invalid Parse.

            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method is called from <see cref="FinishedLaunching(UIApplication, NSDictionary)"/> if defaults are present and valid.
        /// It assigns to the Global variables that are used for local login.
        /// </summary>
        public void SetGlobalsToDefaultsLocalLogin()
        {
            Globals.ServerName = defaultServername;
            Globals.Address = strings.defaultPrefix + defaultIP + ":" + int.Parse(strings.defaultPort);
            Globals.LocalServerinteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address));
        }

        /// <summary>
        /// This method is called from <see cref="FinishedLaunching(UIApplication, NSDictionary)"/> if the default is global login.
        /// It assigns to the Global variables that are used for global login. 
        /// The function HestiaWebServerInteractor.PostUser() function must be called before other functions are called on the WebServerInteractor.
        /// See, <see cref="HestiaWebServerInteractor"/>
        /// </summary>
        public void SetGlobalsToDefaultsGlobalLogin()
        {
            HestiaWebServerInteractor hestiaWebServerInteractor = new HestiaWebServerInteractor(new NetworkHandler(strings.hestiaWebServerAddress, defaultAuth0AccessToken));

            try
            {   
                hestiaWebServerInteractor.PostUser(); 
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while posting user. User possibly already exists.");
                Console.WriteLine(ex);
            }
            // Create an empty list in case no servers can be fetched from the Webserver, to prevent NullReferenceException in Devices main screen
            Globals.Auth0Servers = new System.Collections.Generic.List<backend.models.HestiaServer>();
            try
            {
                Globals.Auth0Servers = hestiaWebServerInteractor.GetServers();
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting servers");
                Console.WriteLine(ex);
                WarningMessage.Display("Exception while getting servers", "Could not get local server list from webserver", Window.RootViewController);
            }
        }

        /// <summary>
        /// This method defines what happens after the application is launched. It initializes SpeechRecognition and retrieves user defaults from memory.
        /// It checks the user defaults to perform the appropriate action. If the previous login was local and the default values are still valid, 
        /// it goes directly to the devices main screen. The same holds for global.
        /// </summary>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Used for UITesting
            Xamarin.Calabash.Start();

            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

            string defaultLocal = userDefaults.StringForKey(strings.defaultsLocalHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(strings.defaultsPortHestia);
            defaultServername = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultAuth0AccessToken = userDefaults.StringForKey(strings.defaultsAccessTokenHestia);

            // The main window where the app lives in
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            // Check if defaults for local/global are present
            if (defaultLocal == bool.TrueString)
            {
                Globals.LocalLogin = true;
                
                // If the server is valid go directly to the Devices main screen
                if(IsServerValid(defaultIP))
                {
                    UINavigationController navigationController = devices2Storyboard.InstantiateViewController(strings.navigationControllerDevicesMain) as UINavigationController;
                    Window.RootViewController = navigationController;
                    // Make key and visible to be able to present possibly Alert window
                    Window.MakeKeyAndVisible();
                    SetGlobalsToDefaultsLocalLogin();
                }
                else
                {   // Server is not valid. Go to server connect screen.
                    Window.RootViewController = devices2Storyboard.InstantiateInitialViewController(); ;
                }
            }
            else if (defaultLocal == bool.FalseString)
            {
                Globals.LocalLogin = false;

                Window.RootViewController = devices2Storyboard.InstantiateViewController(strings.navigationControllerServerSelectList); ;
                // Make key and visible to be able to present possibly Alert window
                Window.MakeKeyAndVisible();
                SetGlobalsToDefaultsGlobalLogin();
            }
            else
            {   // No previous login information available. Go to local/global choose screen.
                Window.RootViewController = mainStoryboard.InstantiateInitialViewController() as UIViewControllerLocalGlobal;
            }
            Window.MakeKeyAndVisible();
            return true;
        }

        /// <summary>
        /// This method is used to handle the call back URL from the Auth0 login
        /// </summary>
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            ActivityMediator.Instance.Send(url.AbsoluteString);
            return true;
        }
    }
}
