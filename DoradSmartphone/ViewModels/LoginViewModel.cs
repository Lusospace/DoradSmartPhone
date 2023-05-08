using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoradSmartphone.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;

        [RelayCommand]
        async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await DisplayLoginError();
            }
            else
            {
                var loginSuccessful = true;
                if(loginSuccessful) { }
                await DisplayLoginError();
            }
        }
        async Task DisplayLoginError()
        {
            await Shell.Current.DisplayAlert("Invalid Attempt", "Invalid Username or Password", "Ok");
            Password = string.Empty;
        }
    }
}
