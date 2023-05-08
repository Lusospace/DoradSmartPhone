using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {
        public LoadingViewModel()
        {
            CheckUserLoginDetais();
        }

        private async void CheckUserLoginDetais()
        {
            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(token)) {
                await GoToLoginPage();
            }
        }

        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }

        private async Task GoToMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
