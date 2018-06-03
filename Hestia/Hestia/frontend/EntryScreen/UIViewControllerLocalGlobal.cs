using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using Hestia.backend.utils;
using Hestia.backend;
using Hestia.Resources;
using Hestia.DevicesScreen.resources;
using Auth0.OidcClient;
using Hestia.backend.authentication;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.frontend;

namespace Hestia
{
    /// <summary>
    /// This view controller belongs to the first window that can be seen when loading the app
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
        string defaultPort;
        string defaultAccessToken;
        
        public UIViewControllerLocalGlobal (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            userDefaults = NSUserDefaults.StandardUserDefaults;
            defaultServerName = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(strings.defaultsPortHestia);
            defaultAccessToken = userDefaults.StringForKey(strings.defaultsAccessTokenHestia);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ToLocalButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                ToLocalScreen();
            };

            ToGlobalButton.TouchUpInside += async (object sender, EventArgs e) =>
            {
                await ToGlobalScreen();
            };         
        }

        bool CheckLocalLoginDefaults()
        {
            if (defaultIP != null)
            {
                return PingServer.Check(strings.defaultPrefix + defaultIP, int.Parse(strings.defaultPort));
            }
            return false;
        }

        bool HasValidGlobalLogin()
        {
            return defaultAccessToken != null;
        }

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

        void ToLocalScreen()
        {
            // Already anticipate local login
            // Check if local serverinformation is present and correct
            bool validIp = CheckLocalLoginDefaults();

            userDefaults.SetString(bool.TrueString, strings.defaultsLocalHestia);
            Globals.LocalLogin = true;

            if (validIp)
            {
                SetValuesAndSegueToDevicesMainLocal();
            }
            else
            {
                UIStoryboard devicesMainStoryboard = UIStoryboard.FromName(strings.devices2StoryBoard, null);
                PresentViewController(devicesMainStoryboard.InstantiateInitialViewController(), true, null);
            }
        }

        async Task ToGlobalScreen()
        {
            userDefaults.SetString(bool.FalseString, strings.defaultsLocalHestia);

            if (HasValidGlobalLogin())
            {
                SetValuesAndSegueToServerSelectGlobal();
            }
            else
            {
                Task<LoginResult> loginResult = GetLoginResult();
                LoginResult logResult = await loginResult;

                Globals.LocalLogin = false;

                if (!logResult.IsError)
                {
                    userDefaults.SetString(logResult.AccessToken, strings.defaultsAccessTokenHestia);
                    SetValuesAndSegueToServerSelectGlobal(logResult.AccessToken);
                }
                else if (!(logResult.Error == "UserCancel"))
                {
                    WarningMessage.Display("Login failed", logResult.Error, this);
                }
            }
        }

        // Sets values in case of defaults presesent
        void SetValuesAndSegueToDevicesMainLocal()
        {
            Globals.LocalLogin = true;
            Globals.ServerName = defaultServerName;
            Globals.Address = strings.defaultPrefix + defaultIP;
            Globals.LocalServerinteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address, int.Parse(strings.defaultPort)));
            PerformSegue(strings.mainToDevicesMain, this);
        }

        // Sets values in case of defaults presesent
        void SetValuesAndSegueToServerSelectGlobal()
        {
            Globals.LocalLogin = false;
            NetworkHandler networkHandler = new NetworkHandler(strings.webserverIP, defaultAccessToken);
            CreateServerInteractorAndSegue(networkHandler);
        }

        //Sets values in case of new login
        void SetValuesAndSegueToServerSelectGlobal(string accessToken)
        {
            userDefaults.SetString(accessToken, strings.defaultsAccessTokenHestia);

            Globals.LocalLogin = false;
            NetworkHandler networkHandler = new NetworkHandler(strings.webserverIP, accessToken);
            CreateServerInteractorAndSegue(networkHandler);
        }

        void CreateServerInteractorAndSegue(NetworkHandler networkHandler)
        {
            HestiaWebServerInteractor hestiaWebServerInteractor = new HestiaWebServerInteractor(networkHandler);
            try
            {
                hestiaWebServerInteractor.PostUser();                
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while posting user. User possibly already exists.");
                Console.WriteLine(ex.StackTrace);
            }
            Globals.Auth0Servers = new List<HestiaServer>();
            try
            {
                List<HestiaServer> servers = hestiaWebServerInteractor.GetServers();
                Globals.Auth0Servers = servers;
                PerformSegue(strings.segueToLocalGlobalToServerSelect, this);
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting servers");
                Console.WriteLine(ex.StackTrace);
                WarningMessage.Display("Exception whle getting server", "Could not get the server information about local server from Auth0 server.", this);
            }
            Console.WriteLine("To Server Select Global");
            PerformSegue(strings.segueToLocalGlobalToServerSelect, this);
        }
    }
}
