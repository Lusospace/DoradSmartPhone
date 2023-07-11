using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class AvatarPage : ContentPage
{
	public AvatarPage(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
	{
		InitializeComponent();
		BindingContext = new AvatarViewModel(glassDTO, toast, bluetoothService);
	}
}