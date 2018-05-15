using Foundation;
using System;
using UIKit;
using Hestia.Resources;
using Auth0.OidcClient;
using Hestia.backend.authentication;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using System.Diagnostics;

namespace Hestia
{
	// TODO remove if local/global is finished
    public partial class UIViewControllerAuth0 : UIViewController
    {
        Auth0Client client;
        NSUserDefaults userDefaults;

        public UIViewControllerAuth0(IntPtr handle) : base(handle)
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Task<LoginResult> loginResult = GetLoginResult();

            //userDefaults.SetString(loginResult.Result.IdentityToken, strings.defaultsIdentityTokenHestia);
            //userDefaults.SetString(loginResult.Result.AccessToken, strings.defaultsAccessTokenHestia);
        
            // PerformSegue...
        }

        public async Task<LoginResult> GetLoginResult()
        {
            client = Auth0Connector.CreateAuthClient(this);
            var loginResult = await client.LoginAsync(new { audience = Resources.strings.apiURL });
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
    }
}
