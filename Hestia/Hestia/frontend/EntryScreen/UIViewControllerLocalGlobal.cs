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
using Hestia.DevicesScreen;
using Hestia.backend.models;
using Hestia.backend.speech_recognition;
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
        private SpeechRecognition speechRecognizer;

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

            SpeechButton.TouchDown += (object sender, EventArgs e) => 
            {
                speechRecognizer = new SpeechRecognition();
                speechRecognizer.StartRecording();
            };

            SpeechButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                string result = speechRecognizer.StopRecording();
                ProcessSpeechResult(result);
            };

            SpeechButton.TouchDragExit += (object sender, EventArgs e) =>
            {
                speechRecognizer.CancelRecording();
            };
        }

        bool CheckLocalLoginDefaults()
        {
            if (defaultIP != null && defaultPort != null)
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

        private async void ProcessSpeechResult(string result)
        {
            string resultLower = result.ToLower();

            if (resultLower.Equals("local"))
            {
                ToLocalScreen();
            }
            else if (resultLower.Equals("global"))
            {
                await ToGlobalScreen();
            }
            else if (resultLower == null)
            {
                new WarningMessage("Something went wrong", "Please make sure you have allowed speech recognition and try again.", this);
            }
            else
            {
                new WarningMessage("Speech could not be recognized", "Please try again.", this);
            }
        }

        private void ToLocalScreen()
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
                Console.WriteLine("To Server Connect screen");
                UIStoryboard devicesMainStoryboard = UIStoryboard.FromName("Devices2", null);
                PresentViewController(devicesMainStoryboard.InstantiateInitialViewController(), true, null);
            }
        }

        private async Task ToGlobalScreen()
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
                    Console.WriteLine("No error during login");
                    userDefaults.SetString(logResult.AccessToken, strings.defaultsAccessTokenHestia);
                    SetValuesAndSegueToServerSelectGlobal(logResult.AccessToken);
                }
                else if (!(logResult.Error == "UserCancel"))
                {
                    new WarningMessage("Login failed", logResult.Error, this);
                }
            }
        }

        // Sets values in case of defaults presesent
        void SetValuesAndSegueToDevicesMainLocal()
        {
            Globals.LocalLogin = true;
            Globals.ServerName = defaultServerName;
            Globals.Address = defaultIP;
            Globals.LocalServerinteractor = new HestiaServerInteractor(new NetworkHandler(Globals.Address, int.Parse(strings.defaultPort)));
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
                Console.WriteLine("Exception while posting user. User possibly already exists.");
                Console.WriteLine(ex.StackTrace);
            }
            Globals.Auth0Servers = new List<HestiaServer>();
            try
            {
                List<HestiaServer> servers = hestiaWebServerInteractor.GetServers();
                Globals.Auth0Servers = servers;
                Console.WriteLine("number of servers:" + Globals.Auth0Servers.Count);
            }
            catch(ServerInteractionException ex)
            {
                Console.WriteLine("Exception while getting servers");
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("To Server Select Global");
            PerformSegue(strings.segueToLocalGlobalToServerSelect, this);
        }
    }
}
