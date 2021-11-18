using AppProjekt.Auth;
using System.Threading.Tasks;

namespace AppProjekt.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Authenticate();
        AuthenticationResult AuthenticationResult { get; }
        Task Logout();
    }
}
