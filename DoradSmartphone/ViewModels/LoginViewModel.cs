using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Services;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly LoginService loginService;

        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;

        public LoginViewModel(LoginService loginService)
        {
            Title = "Login";
            this.loginService = loginService;
        }

        [RelayCommand]
        public async Task Login()
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))            
                await DisplayLoginError();
            else
            {
                await loginService.VerifyLogin(username, password);
                // Clear the username and password fields
                username = string.Empty;
                password = string.Empty;
            }

        }
        async Task DisplayLoginError()
        {
            await Shell.Current.DisplayAlert("Invalid Attempt", "Invalid Username or Password", "Ok");            
        }

        [RelayCommand]
        async Task User() => await Shell.Current.GoToAsync(nameof(UserPage));

    }
}
