using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Services;
using DoradSmartphone.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoradSmartphone.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly LoginService _loginService;

        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;

        public LoginViewModel(LoginService loginService)
        {
            Title = "Login";
            _loginService = loginService;
        }

        [RelayCommand]
        async Task Login()
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayLoginError();
            }
            else
            {
                await _loginService.VerifyLogin(username, password);
                await Shell.Current.GoToAsync($"{nameof(MainPage)}");
            }
        }
        async Task DisplayLoginError()
        {
            await Shell.Current.DisplayAlert("Invalid Attempt", "Invalid Username or Password", "Ok");
            password = string.Empty;
        }

        [RelayCommand]
        async Task User() => await Shell.Current.GoToAsync(nameof(UserPage));

    }
}
