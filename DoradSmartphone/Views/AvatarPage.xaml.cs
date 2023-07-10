using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class AvatarPage : ContentPage
{
	public AvatarPage(GlassDTO glassDTO)
	{
		InitializeComponent();
		BindingContext = new AvatarViewModel(glassDTO);
	}
}