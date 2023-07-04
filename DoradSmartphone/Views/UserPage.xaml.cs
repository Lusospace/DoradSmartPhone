using Android.Views.InputMethods;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class UserPage : ContentPage
{
    private readonly UserViewModel _userViewModel;
	public UserPage(UserViewModel userViewModel)
	{
		InitializeComponent();
		this.BindingContext = userViewModel;
        _userViewModel = userViewModel;
	}

    private void OnEntryCompleted(object sender, System.EventArgs e)
    {
        HideKeyboard();
        if(Name.Text.Length > 0 && Email.Text.Length > 0 && Password.Text.Length > 0 && Phonenumber.Text.Length > 0)
            _ = _userViewModel.CreateUser();
    }

    private void HideKeyboard()
    {
        var inputMethodManager = (InputMethodManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.InputMethodService);
        var activity = Platform.CurrentActivity;
        var windowToken = activity.CurrentFocus?.WindowToken;
        inputMethodManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
    }
}