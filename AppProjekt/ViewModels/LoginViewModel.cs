using System;
using System.Collections.Generic;
using System.Text;
using AppProjekt.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppProjekt.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;

        public Command LoginCommand { get; }
        public Command LogOutCommand { get; }

        public LoginViewModel()
        {
            _authenticationService = DependencyService.Get<IAuthenticationService>();
            LoginCommand = new Command(OnLoginClicked);
            LogOutCommand = new Command(OnLogoutClicked);
        }


        private async void OnLoginClicked()
        {
            var authenticationResult = await _authenticationService.Authenticate();
            if (!authenticationResult.IsError)
            {
                await SecureStorage.SetAsync("accessToken", authenticationResult.AccessToken);
            }

            await Shell.Current.GoToAsync("TelemetricsPage");
        }

        private async void OnLogoutClicked()
        {
            await _authenticationService.Logout();
            SecureStorage.Remove("accessToken");
        }
    }
}
