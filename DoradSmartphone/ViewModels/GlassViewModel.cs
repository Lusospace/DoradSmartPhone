using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Services.Bluetooth;
using System.ComponentModel;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class GlassViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string battery;
        [ObservableProperty]
        private string status;

        private IToast toast;

        public GlassViewModel(IToast toast)
        {
            Title = "Glasses";
            this.toast = toast;
            CheckConnection();
        }

        public void CheckConnection()
        {
            BluetoothService bluetoothService = new BluetoothService(toast); // Instantiate BluetoothService without passing a toast object

            int connectionState = bluetoothService.GetState();

            if (connectionState == BluetoothService.STATE_CONNECTED)
            {
                Status = "Connected";
            }
            else
            {
                Status = "Disconnected";
            }
        }

        [RelayCommand]
        public void Calibration()
        {
            CheckConnection();
        }

    }
}
