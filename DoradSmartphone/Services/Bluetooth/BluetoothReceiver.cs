using Android.Bluetooth;
using Android.Content;
using DoradSmartphone.Models;

namespace DoradSmartphone.Services.Bluetooth
{
    public class BluetoothReceiver : BroadcastReceiver
    {
        public List<DeviceCandidate> Devices { get; } = new List<DeviceCandidate>();

        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;
            if (action == BluetoothDevice.ActionFound)
            {
                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                if (device != null)
                {
                    Devices.Add(new DeviceCandidate { 
                        Name = device.Name, 
                        Address = device.Address 
                    });
                }
            }
        }
    }
}
