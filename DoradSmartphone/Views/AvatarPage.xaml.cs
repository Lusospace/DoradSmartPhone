using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class AvatarPage : ContentPage
{
	public AvatarPage(AvatarViewModel avatarViewModel)
	{
		InitializeComponent();
		BindingContext = avatarViewModel;
	}
}