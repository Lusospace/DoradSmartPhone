using Android.Bluetooth;

namespace DoradSmartphone.Services.Bluetooth
{
    public interface IBluetoothService
    {
        int GetState();
        void Start();
        void Accept();
        void Connect(BluetoothDevice device);
        void Write(byte[] data);
        void Stop();
    }
}