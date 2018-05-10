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
        }

		public override void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);

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

            ToGlobalButton.TouchUpInside += delegate (object sender, EventArgs e)
            {
                Globals.LocalLogin = false;
                userDefaults.SetString(bool.FalseString, strings.defaultsLocalHestia);

                Console.WriteLine("To auth0login");
                Task<LoginResult> loginResult = GetLoginResult();

                if (HasValidGlobalLogin())
                {
                    SetValuesAndSegueToServerSelect();
                }

                userDefaults.SetString(loginResult.Result.IdentityToken, strings.defaultsIdentityTokenHestia);
                userDefaults.SetString(loginResult.Result.AccessToken, strings.defaultsAccessTokenHestia);

                SetValuesAndSegueToServerSelect();
            };
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

        public void CalledFromAppDelegate()
        {
            Task<LoginResult> loginResult = GetLoginResult();
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

        void SetValuesAndSegueToServerSelect()
        {
            Globals.LocalLogin = false;
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
