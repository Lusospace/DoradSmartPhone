using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Services.Bluetooth;

namespace DoradSmartphone.ViewModels
{
    public partial class GlassViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string battery;
        [ObservableProperty]
        private string status;        

        public GlassViewModel()
        {
            Title = "Glasses";            
            CheckConnection();
        }

        public void CheckConnection()
        {
            BluetoothService bluetoothService = new BluetoothService();

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
