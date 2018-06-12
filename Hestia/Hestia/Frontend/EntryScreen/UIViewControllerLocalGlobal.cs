using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using Hestia.Resources;
using Auth0.OidcClient;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using Hestia.Backend.Authentication;
using Hestia.Frontend.Resources;
using Hestia.Backend;
using Hestia.Backend.Exceptions;
using Hestia.Backend.Models;

namespace Hestia.Frontend.EntryScreen
{
    /// <summary>
    /// This ViewController belongs to the first window that can be seen when loading the app
    /// if no user default for local/global is present. The user can then choose local/global.
    /// </summary>
    public partial class UIViewControllerLocalGlobal : UIViewController
    {
        Auth0Client client;
        const int IconDimension = 50;
        const int BottomPadding = 50;

        // User defaults
        NSUserDefaults userDefaults;
        string defaultServerName;
        string defaultIP;
        string defaultAccessToken;
        
        public UIViewControllerLocalGlobal (IntPtr handle) : base (handle)
        {
        }

        /// <summary>
        /// This method is called if the View did appear, the user defaults are retrieved from the iPhone's memory.
        /// They are used if for example one disconnects from Auth0 and then reconnects to a local server. 
        /// Then the local server that was still saved as default is connected to.
        /// Furthermore, the actions of the buttons are set.
        /// </summary>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            userDefaults = NSUserDefaults.StandardUserDefaults;
            defaultServerName = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
            defaultAccessToken = userDefaults.StringForKey(strings.defaultsAccessTokenHestia);

            ToLocalButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                ToLocalScreen();
            };

            ToGlobalButton.TouchUpInside += async (object sender, EventArgs e) =>
            {
                await ToGlobalScreen();
            };         
        }

        /// <summary>
        /// Checks if the Global login is valid. It only has to check that the accesstoken is not null, because
        /// accesstokens remain valid once they are created.
        /// </summary>
        bool HasValidGlobalLogin()
        {
            return defaultAccessToken != null;
        }

        /// <summary>
        /// This method launches the Auth0 login screen.
        /// </summary>
        public async Task<LoginResult> GetLoginResult()
        {
            client = Auth0Connector.CreateAuthClient(this);
            var loginResult = await client.LoginAsync(new { audience = strings.apiURL });
            if (loginResult.IsError)
            {
                Console.WriteLine($"An error occurred during login: {loginResult.Error}");
            }
            return loginResult;
        }

        /// <summary>
        /// This methods handles the click on the local button. It checks if default information is valid, 
        /// if it is valid it loads the Devices main screen, otherwise it goes to the Server connect screen.
        /// </summary>
        void ToLocalScreen()
        {
            // Check if local serverinformation is present and correct
            bool validIp = AppDelegate.IsServerValid(defaultIP);

            userDefaults.SetString(bool.TrueString, strings.defaultsLocalHestia);
            Globals.LocalLogin = true;

            if (validIp) // Go to Devices main
            {
                SetValuesAndSegueToDevicesMainLocal();
            }
            else // Go to Server select
            {
                UIStoryboard devicesMainStoryboard = UIStoryboard.FromName(strings.devices2StoryBoard, null);
                PresentViewController(devicesMainStoryboard.InstantiateInitialViewController(), true, null);
            }
        }

        /// <summary>
        /// This methods handles the click on the global button. It checks if default information is valid, 
        /// if it is valid is loads the Devices main screen, otherwise it presents the Auth0 login screen.
        /// </summary>
        async Task ToGlobalScreen()
        {
            userDefaults.SetString(bool.FalseString, strings.defaultsLocalHestia);

            if (HasValidGlobalLogin()) // Go to Devices main
            {
                SetValuesAndSegueToServerSelectGlobal();
            }
            else // Present Auth0 login
            {
                Task<LoginResult> loginResult = GetLoginResult();
                LoginResult logResult = await loginResult;

                Globals.LocalLogin = false;

                if (!logResult.IsError)
                {
                    userDefaults.SetString(logResult.AccessToken, strings.defaultsAccessTokenHestia);
                    SetValuesAndSegueToServerSelectGlobal(logResult.AccessToken);
                }
                // Do not display a warning if user click back to the local/global screen
                else if (!(logResult.Error == "UserCancel"))
                {
                    WarningMessage.Display("Login failed", logResult.Error, this);
                }
            }
        }

        /// <summary>
        /// Sets values to default values and go to Devices main screen
        /// </summary>
        void SetValuesAndSegueToDevicesMainLocal()
        {
            Globals.LocalLogin = true;
            Globals.ServerName = defaultServerName;
            Globals.Address = strings.defaultPrefix + defaultIP + ":" + int.Parse(strings.defaultPort);
            Globals.LocalServerinteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address));
            PerformSegue(strings.mainToDevicesMain, this);
        }

        /// <summary>
        /// Sets values to default values and go to Devices main screen
        /// </summary>
        void SetValuesAndSegueToServerSelectGlobal()
        {
            Globals.LocalLogin = false;
            NetworkHandler networkHandler = new NetworkHandler(strings.hestiaWebServerAddress, defaultAccessToken);
            CreateServerInteractorAndSegue(networkHandler);
        }

        /// <summary>
        /// Sets values to new login values and go to Devices main screen
        /// </summary>
        void SetValuesAndSegueToServerSelectGlobal(string accessToken)
        {
            userDefaults.SetString(accessToken, strings.defaultsAccessTokenHestia);

            Globals.LocalLogin = false;
            Globals.HestiaWebserverNetworkHandler = new NetworkHandler(strings.hestiaWebServerAddress, accessToken);
            CreateServerInteractorAndSegue(Globals.HestiaWebserverNetworkHandler);
        }

        /// <summary>
        /// Creates the HestiaWebServerInteractor to communicate with the Webserver. It posts the new user and fetches
        /// the list of Local servers from the Webserver. It this succeeds, the Select server screen is shown.
        /// </summary>
        void CreateServerInteractorAndSegue(NetworkHandler networkHandler)
        {
            Globals.HestiaWebServerInteractor = new HestiaWebServerInteractor(Globals.HestiaWebserverNetworkHandler);
            try
            {
                Globals.HestiaWebServerInteractor.PostUser();                
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while posting user. User possibly already exists.");
                Console.WriteLine(ex);
            }
            Globals.Auth0Servers = new List<HestiaServer>();
            try
            {
                List<HestiaServer> servers = Globals.HestiaWebServerInteractor.GetServers();
                Globals.Auth0Servers = servers;
                PerformSegue(strings.segueToLocalGlobalToServerSelect, this);
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting servers");
                Console.WriteLine(ex);
                WarningMessage.Display("Exception whle getting server", "Could not get the server information about local server from Auth0 server.", this);
            }

            PerformSegue(strings.segueToLocalGlobalToServerSelect, this);
        }
    }
}
