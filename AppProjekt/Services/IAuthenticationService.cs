using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppProjekt.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Authenticate();

        Task Logout();
    }
}
