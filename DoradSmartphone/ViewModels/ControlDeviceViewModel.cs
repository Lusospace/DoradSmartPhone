using Android.Bluetooth;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;

namespace DoradSmartphone.ViewModels
{
    public partial class ControlDeviceViewModel : BaseViewModel
    {
        private readonly IBluetoothService bluetoothService;

        public ControlDeviceViewModel()
        {
            Title = "Send Comands";
            bluetoothService = ServiceLocator.Get<IBluetoothService>();
        }

        [RelayCommand]
        public void Start()
        {
            var command = new CommandDTO() { Command = Constants.STARTRUN };
            SendOverBluetooth(command);
        }

        [RelayCommand]
        public void Stop()
        {
            var command = new CommandDTO() { Command = Constants.STOPRUN };
            SendOverBluetooth(command);
        }

        private void SendOverBluetooth(CommandDTO command) => bluetoothService.Write(ConvertToJsonAndBytes.Convert(command));
    }    
}
