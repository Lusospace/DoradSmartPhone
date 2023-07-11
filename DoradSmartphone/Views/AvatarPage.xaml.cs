using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class AvatarPage : ContentPage
{
	public AvatarPage(GlassDTO glassDTO, IToast toast)
	{
		InitializeComponent();
		BindingContext = new AvatarViewModel(glassDTO, toast);
	}
}