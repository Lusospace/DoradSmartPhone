using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class DisplaySelectedItemsPage : ContentPage
{    

    public DisplaySelectedItemsPage(TransferDTO transferDTO, IBluetoothService bluetoothService)
    {
        InitializeComponent();        
        BindingContext = new DisplaySelectedItemsViewModel(transferDTO, bluetoothService);
    }
}