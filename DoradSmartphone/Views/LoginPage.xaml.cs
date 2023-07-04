using Android.Views.InputMethods;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel loginViewModel;
	public LoginPage(LoginViewModel loginPageViewModel)
	{
		InitializeComponent();
		this.BindingContext = loginPageViewModel;
        loginViewModel = loginPageViewModel;

        MainLayout.Unfocused += OnMainLayoutUnfocused;
    }

    private void OnMainLayoutUnfocused(object sender, FocusEventArgs e)
    {
        // Unfocus any focused elements to hide the keyboard
        Username?.Unfocus();
        Password?.Unfocus();
    }

    private void OnEntryCompleted(object sender, System.EventArgs e)
    {
        HideKeyboard();
        if(Username.Text != null && Password.Text != null)
            _ = loginViewModel.Login();
    }

    private void HideKeyboard()
    {
        var inputMethodManager = (InputMethodManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.InputMethodService);
        var activity = Platform.CurrentActivity;
        var windowToken = activity.CurrentFocus?.WindowToken;
        inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
    }
}