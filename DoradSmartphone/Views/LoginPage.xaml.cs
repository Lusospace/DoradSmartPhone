using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginPageViewModel)
	{
		InitializeComponent();
		this.BindingContext= loginPageViewModel;
	}
}