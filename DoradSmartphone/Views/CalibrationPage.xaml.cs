using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class CalibrationPage : ContentPage
{
	public CalibrationPage(byte[] photo)
	{
		InitializeComponent();
		BindingContext = new CalibrationViewModel(photo);
	}
}