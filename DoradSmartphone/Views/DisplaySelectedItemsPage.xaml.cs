using AndroidX.Lifecycle;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class DisplaySelectedItemsPage : ContentPage
{    

    public DisplaySelectedItemsPage(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
    {
        InitializeComponent();        
        BindingContext = new DisplaySelectedItemsViewModel(glassDTO, toast, bluetoothService);
    }
}