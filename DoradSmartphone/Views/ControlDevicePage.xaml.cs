using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ControlDevicePage : ContentPage
{
	public ControlDevicePage()
	{
		InitializeComponent();
		BindingContext = new ControlDeviceViewModel();
	}
}