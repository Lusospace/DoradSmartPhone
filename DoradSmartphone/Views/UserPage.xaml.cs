using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class UserPage : ContentPage
{
	public UserPage(UserViewModel userViewModel)
	{
		InitializeComponent();
		this.BindingContext = userViewModel;
	}
}