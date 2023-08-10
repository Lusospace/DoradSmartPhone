using DoradSmartphone.Services.Bluetooth;

namespace DoradSmartphone.Services
{
    public class MessageService
    {
        private BluetoothService bluetoothService;
        public MessageService()
        {
            bluetoothService = new BluetoothService();
            bluetoothService.DataReceived += BluetoothService_DataReceived;
        }

        private void BluetoothService_DataReceived(string receivedData)
        {            
            
        }
    }
}
