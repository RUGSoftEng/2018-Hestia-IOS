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
using System.Diagnostics;
using Hestia.backend.exceptions;

namespace Hestia
{
    /// <summary>
    /// This view controller belongs to the first window that can be seen when loading the app
    /// if no user default for local/global is present. The user can then choose local/global.
    /// </summary>
    public partial class UIViewControllerLocalGlobal : UIViewController
    {
        Auth0Client client;

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

            // Already anticipate local login
            // Check if local serverinformation is present and correct
            bool validIp = CheckLocalLoginDefaults();

            ToLocalButton.TouchUpInside += delegate
            {
                userDefaults.SetString(bool.TrueString, strings.defaultsLocalHestia);
                Globals.LocalLogin = true;

                if (validIp)
                {
                    SetValuesAndSegueToDevicesMainLocal();
                }
                else
                {
                    Console.WriteLine("To Server Connect screen");
                    PerformSegue(strings.mainToServerConnect, this);
                }
            };

            ToGlobalButton.TouchUpInside += async delegate
            {
                if (HasValidGlobalLogin())
                {
                    SetValuesAndSegueToServerSelectGlobal();
                }
                else
                {
                    Task<LoginResult> loginResult = GetLoginResult();
                    LoginResult logResult = await loginResult;
                    HandleGlobalButtonTouchResult(logResult);
                }
            };
        }

        void HandleGlobalButtonTouchResult(LoginResult loginResult)
        {
            Globals.LocalLogin = false;
            userDefaults.SetString(bool.FalseString, strings.defaultsLocalHestia);

            if (!loginResult.IsError)
            {
                Console.WriteLine("No error during login");
                userDefaults.SetString(loginResult.AccessToken, strings.defaultsAccessTokenHestia);
                SetValuesAndSegueToServerSelectGlobal(loginResult.AccessToken);
            }
            else if(!(loginResult.Error == "UserCancel"))
            {
                DisplayWarningMessage(loginResult.Error);
            }
         }

        void DisplayWarningMessage(string error)
        {
            string title = "Login failed";
            string message = error;
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
        }

        bool CheckLocalLoginDefaults()
        {
            if (defaultServerName != null && defaultIP != null && defaultPort != null)
            {
                return PingServer.Check(defaultIP, int.Parse(defaultPort));
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
                Debug.WriteLine($"An error occurred during login: {loginResult.Error}");
            }
            else
            {
                Debug.WriteLine($"id_token: {loginResult.IdentityToken}");
                Debug.WriteLine($"access_token: {loginResult.AccessToken}");
            }
            return loginResult;
        }

        // Sets values in case of defaults presesent
        void SetValuesAndSegueToDevicesMainLocal()
        {
            Globals.LocalLogin = true;
            Globals.ServerName = defaultServerName;
            Globals.IP = defaultIP;
            Globals.Port = int.Parse(defaultPort);
            Globals.LocalServerinteractor = new HestiaServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
            Console.WriteLine("To Devices Main Local");
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
                Console.WriteLine(ex.StackTrace);
            }

            try
            {
                Globals.Auth0Servers = hestiaWebServerInteractor.GetServers();
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("To Server Select Global");
            PerformSegue(strings.segueToLocalGlobalToServerSelect, this);
        }
	}
}
