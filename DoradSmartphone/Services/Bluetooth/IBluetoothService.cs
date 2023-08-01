using Android.Bluetooth;

namespace DoradSmartphone.Services.Bluetooth
{
    public interface IBluetoothService
    {
        int GetState();
        Task Start();
        void Accept();        
        void Write(byte[] data);
        bool CheckConnection();
        event EventHandler<bool> ConnectionStatusChanged;
    }
}