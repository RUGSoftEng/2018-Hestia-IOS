using Foundation;
using System;
using UIKit;
using Hestia.backend.authentication;
using Auth0.OidcClient;
using System.Threading.Tasks;
using IdentityModel.OidcClient;

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
            GetLoginResult();

		}

        public async Task<LoginResult> GetLoginResult() {
            var loginResult = await client.LoginAsync();

        }
	}
}