using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        [RelayCommand]
        async void Logout()
        {
            Preferences.Clear();
            //Shell.SetTabBarIsVisible(Application.Current.MainPage, false);
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
