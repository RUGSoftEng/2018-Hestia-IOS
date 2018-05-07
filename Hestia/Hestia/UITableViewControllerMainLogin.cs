using Foundation;
using System;
using UIKit;
using Hestia.backend.authentication;
using Auth0.OidcClient;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using System.Diagnostics;

namespace Hestia
{
    public partial class UITableViewControllerMainLogin : UIViewController
    {
        private Auth0Client client;

        public UITableViewControllerMainLogin (IntPtr handle) : base (handle)
        {
            client = Auth0Connector.CreateAuthClient(this);
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            Task<LoginResult> loginResult = GetLoginResult();

		}

        public async Task<LoginResult> GetLoginResult() {
            var loginResult = await client.LoginAsync();
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