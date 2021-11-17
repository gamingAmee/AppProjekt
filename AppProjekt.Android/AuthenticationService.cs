using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppProjekt.Services;
using Xamarin.Forms;
using AppProjekt.Droid;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AuthenticationService))]

namespace AppProjekt.Droid
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuth0Client _auth0Client;

        public AuthenticationService()
        {
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "dev-1odmmset.eu.auth0.com",
                ClientId = "6LMuzG7j9c7qbxIe6XVdAlLHr46Lj7wd"
            });
        }

        public Task<LoginResult> Authenticate()
        {
            return _auth0Client.LoginAsync();
        }
        public async Task Logout()
        {
            await _auth0Client.LogoutAsync();
        }
    }


}
