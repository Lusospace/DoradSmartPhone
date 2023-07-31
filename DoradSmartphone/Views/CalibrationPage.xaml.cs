using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class CalibrationPage : ContentPage
{
	public CalibrationPage(byte[] photo, IBluetoothService bluetoothService)
	{
		InitializeComponent();
		BindingContext = new CalibrationViewModel(photo, bluetoothService);
	}
}