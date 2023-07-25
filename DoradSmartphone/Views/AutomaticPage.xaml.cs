using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class AutomaticPage : ContentPage
{
    public AutomaticPage(TransferDTO transferDTO, IBluetoothService bluetoothService)
    {
        InitializeComponent();
        BindingContext = new AutomaticViewModel(transferDTO, bluetoothService);
    }
}