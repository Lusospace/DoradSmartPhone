using DoradSmartphone.DTO;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class GeneralPage : ContentPage
{
	public GeneralPage(TransferDTO transferDTO)
	{
		InitializeComponent();
		BindingContext = new GeneralViewModel(transferDTO);
    }
}