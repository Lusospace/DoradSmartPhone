using DoradSmartphone.DTO;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class GeneralPage : ContentPage
{
	public GeneralPage(GlassDTO glassDTO)
	{
		InitializeComponent();
		BindingContext = new GeneralViewModel(glassDTO);
    }
}