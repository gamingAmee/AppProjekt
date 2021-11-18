using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AppProjekt.Auth;
using AppProjekt.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppProjekt.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginViewModel()
        {
            _authenticationService = DependencyService.Get<IAuthenticationService>();
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        Command loginCommand;
        public Command LoginCommand => loginCommand
            ?? (loginCommand = new Command(async () =>
            {
                AuthenticationResult authenticationResult = await _authenticationService.Authenticate();
                if (!authenticationResult.IsError)
                {
                    await SecureStorage.SetAsync("accessToken", authenticationResult.AccessToken);
                    IsLoggedIn = true;
                }

                await Shell.Current.GoToAsync($"//TelemetricsPage");
            }));

        Command logoutCommand;
        public Command LogoutCommand => logoutCommand
            ?? (logoutCommand = new Command(async () =>
            {
                await _authenticationService.Logout();
                SecureStorage.Remove("accessToken");
                IsLoggedIn = false;
            }));
    }
}
