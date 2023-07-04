using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

