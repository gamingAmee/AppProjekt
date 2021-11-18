using Android.OS;
using AppProjekt.Services;
using Xamarin.Forms;
using AppProjekt.Droid;
using Auth0.OidcClient;
using System.Diagnostics;
using IdentityModel.OidcClient;
using System.Threading.Tasks;
using AppProjekt.Auth;

[assembly: Dependency(typeof(AuthenticationService))]
namespace AppProjekt.Droid
{
    public class AuthenticationService : IAuthenticationService
    {
        private Auth0Client _auth0Client;

        public AuthenticationService()
        {
            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = AuthConfig.Domain,
                ClientId = AuthConfig.ClientId
            });
        }
        public AuthenticationResult AuthenticationResult { get; private set; }

        public async Task<AuthenticationResult> Authenticate()
        {
            LoginResult auth0LoginResult = await _auth0Client.LoginAsync(new { audience = AuthConfig.Audience });

            AuthenticationResult authenticationResult;

            if (!auth0LoginResult.IsError)
            {
                authenticationResult = new AuthenticationResult()
                {
                    AccessToken = auth0LoginResult.AccessToken,
                    IdToken = auth0LoginResult.IdentityToken,
                    UserClaims = auth0LoginResult.User.Claims
                };
            }
            else
                authenticationResult = new AuthenticationResult(auth0LoginResult.IsError, auth0LoginResult.Error);

            AuthenticationResult = authenticationResult;
            return authenticationResult;
        }

        public async Task Logout()
        {
            await _auth0Client.LogoutAsync();
        }
    }


}
