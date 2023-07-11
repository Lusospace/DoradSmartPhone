using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class AutomaticPage : ContentPage
{
    public AutomaticPage(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
    {
        InitializeComponent();
        BindingContext = new AutomaticViewModel(glassDTO, toast, bluetoothService);
    }
}