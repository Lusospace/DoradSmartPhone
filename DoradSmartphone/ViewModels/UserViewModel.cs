using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Services;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        //private Itoast
        private readonly UserService userService;

        [ObservableProperty]
        string name;
        [ObservableProperty]
        string email;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        string phoneNumber;

        public UserViewModel(UserService userService)
        {
            Title = "User Registration";
            this.userService = userService;
        }

        [RelayCommand]
        public async Task CreateUser()
        {
            if (string.IsNullOrEmpty(name))
            {
                await DisplayLoginError("Type a name", "Ok");
            }
            else if (string.IsNullOrEmpty(email))
            {
                await DisplayLoginError("Type an email", "Ok");
            }
            else if (string.IsNullOrEmpty(password))
            {
                await DisplayLoginError("Type a password", "Ok");
            }
            else if (string.IsNullOrEmpty(phoneNumber))
            {
                await DisplayLoginError("Type a phone number", "Ok");
            } else
            {
                var result = await userService.SaveUser(name, email, password, phoneNumber);
                if (result)
                {
                    Name = string.Empty; 
                    Email = string.Empty;
                    Password = string.Empty;
                    PhoneNumber = string.Empty;
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
        }

        async Task DisplayLoginError(string msg, string action) => await Shell.Current.DisplayAlert("Invalid Attempt", msg, action);
    }
}
