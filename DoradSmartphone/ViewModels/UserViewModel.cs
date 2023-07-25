using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {        
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
                Toaster.MakeToast("Type a name");
            }
            else if (string.IsNullOrEmpty(email))
            {
                Toaster.MakeToast("Type an email");
            }
            else if (string.IsNullOrEmpty(password))
            {
                Toaster.MakeToast("Type a password");
            }
            else if (string.IsNullOrEmpty(phoneNumber))
            {
                Toaster.MakeToast("Type a phone number");
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
    }
}
