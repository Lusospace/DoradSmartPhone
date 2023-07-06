using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class GlassPage : ContentPage
{
	public GlassPage(GlassViewModel glassViewModel)
	{
        InitializeComponent();
		BindingContext = glassViewModel;
	}
}