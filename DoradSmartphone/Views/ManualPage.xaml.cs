using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ManualPage : ContentPage
{
    private Point initialPosition;

    public ManualPage(TransferDTO transferDTO, IBluetoothService bluetoothService)
    {
        InitializeComponent();        
        BindingContext = new ManualViewModel(transferDTO, bluetoothService);
    }
}