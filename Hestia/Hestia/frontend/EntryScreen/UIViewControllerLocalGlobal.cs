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

namespace Hestia
{   // This view controller belongs to the first window that can be shown on loading the app
    // if no user default for local/global is present. The user can then choose local/global.
    public partial class UIViewControllerLocalGlobal : UIViewController
    {
        Auth0Client client;

        // User defaults
        NSUserDefaults userDefaults;
        string defaultServerName;
        string defaultIP;
        string defaultPort;
        string defaultAccessToken;
        string defaultIdentityToken;

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
            defaultIdentityToken = userDefaults.StringForKey(strings.defaultsIdentityTokenHestia);

            // Already anticipate local login
            // Check if local serverinformation is present and correct
            bool validIp = CheckLocalLoginDefaults();

            ToLocalButton.TouchUpInside += delegate (object sender, EventArgs e)
            {
                userDefaults.SetString(bool.TrueString, strings.defaultsLocalHestia);
                Globals.LocalLogin = true;

                if (validIp)
                {
                    SetValuesAndSegueToDevicesMainLocal();
                }
                else
                {
                    Console.WriteLine("To serverconnect");
                    PerformSegue(strings.mainToServerConnect, this);
                }
            };

            ToGlobalButton.TouchUpInside += async delegate (object sender, EventArgs e)
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
                Console.WriteLine("No error");
                userDefaults.SetString(loginResult.IdentityToken, strings.defaultsIdentityTokenHestia);
                userDefaults.SetString(loginResult.AccessToken, strings.defaultsAccessTokenHestia);
                // TODO edit values in to serverselect
                SetValuesAndSegueToServerSelectGlobal();
            }
            else if(!(loginResult.Error == "UserCancel"))
            {
                string title = "Login failed";
                string message = loginResult.Error;
                var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
            }
         }

        bool CheckLocalLoginDefaults()
        {
            bool validIp = false;

            if (defaultServerName != null && defaultIP != null && defaultPort != null)
            {
                validIp = PingServer.Check(defaultIP, int.Parse(defaultPort));
            }

            return validIp;
        }

        public bool HasValidGlobalLogin()
        {
            //TODO back end method that checks validity of token
            return false;
        }

        public async Task CalledFromAppDelegateAsync()
        {
            Console.Write("Called from appdelegate");
            Task<LoginResult> loginResult = GetLoginResult();
            LoginResult logResult = await loginResult;
            HandleGlobalButtonTouchResult(logResult);
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

        void SetValuesAndSegueToDevicesMainLocal()
        {
            Globals.LocalLogin = true;
            Globals.ServerName = defaultServerName;
            Globals.IP = defaultIP;
            Globals.Port = int.Parse(defaultPort);
            Globals.LocalServerinteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
            Console.WriteLine("To devices main local");
            PerformSegue(strings.mainToDevicesMain, this);
        }

        void SetValuesAndSegueToServerSelectGlobal()
        {
            Globals.LocalLogin = false;
            Globals.Auth0Servers = new List<WebServer>();
            List<ServerInteractor> serverInteractors = new List<ServerInteractor>();
            // TODO Backend method that gets Auth0Servers
            //

            //foreach (WebServer auth0server in Auth0Servers)
            //{
            //     serverInteractors.Add(auth0server.Interactor);
            //}

            Console.WriteLine("To server select global");
            PerformSegue("localGlobalToServerSelect", this);
        }
	}
}
