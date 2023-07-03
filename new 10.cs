using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Util;
using Java.Util;
using Newtonsoft.Json;

namespace com.example.yourappname
{
    public class DashboardViewModel : ViewModel
    {
        const string TAG = "DashboardViewModel";
        const string DEVICE_NAME = "Your Device Name";
        const string JSON_FILE_PATH = "path/to/your/json/file.json";

        BluetoothAdapter btAdapter;
        BluetoothChatService btService;
        BluetoothDevice btDevice;
        BluetoothSocket btSocket;
        Handler handler;
        Context context;
        bool isConnected = false;

        public DashboardViewModel()
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
            handler = new MyHandler(this);
        }

        public void ConnectToDevice()
        {
            if (btAdapter == null || !btAdapter.IsEnabled)
            {
                // Bluetooth is not supported or not enabled on the device
                // Handle this case accordingly
                return;
            }

            // Check if the device is already paired
            btDevice = null;
            foreach (var device in btAdapter.BondedDevices)
            {
                if (device.Name == DEVICE_NAME)
                {
                    btDevice = device;
                    break;
                }
            }

            if (btDevice == null)
            {
                // The device is not paired
                // Handle this case accordingly
                return;
            }

            btService = new BluetoothChatService(handler);
            btService.Start();
            btService.Connect(btDevice);
        }

        public void SendJsonFile()
        {
            if (btService == null || btService.GetState() != BluetoothChatService.STATE_CONNECTED)
            {
                // The device is not connected
                // Handle this case accordingly
                return;
            }

            // Read the JSON file
            string json;
            using (StreamReader reader = new StreamReader(context.Assets.Open(JSON_FILE_PATH)))
            {
                json = reader.ReadToEnd();
            }

            // Convert the JSON string to bytes
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

            // Send the JSON bytes
            btService.Write(jsonBytes);
        }

        public void DisconnectFromDevice()
        {
            if (btService != null)
            {
                btService.Stop();
                btService = null;
            }
        }

        class MyHandler : Handler
        {
            const string TAG = "MyHandler";
            DashboardViewModel viewModel;

            public MyHandler(DashboardViewModel viewModel)
            {
                this.viewModel = viewModel;
            }

            public override void HandleMessage(Message msg)
            {
                switch (msg.What)
                {
                    case Constants.MESSAGE_STATE_CHANGE:
                        int newState = msg.Arg1;
                        viewModel.HandleStateChange(newState);
                        break;
                    case Constants.MESSAGE_DEVICE_NAME:
                        string deviceName = msg.Data.GetString(Constants.DEVICE_NAME);
                        viewModel.HandleDeviceName(deviceName);
                        break;
                    case Constants.MESSAGE_TOAST:
                        string toastMessage = msg.Data.GetString(Constants.TOAST);
                        viewModel.HandleToastMessage(toastMessage);
                        break;
                    case Constants.MESSAGE_READ:
                        byte[] readBytes = (byte[])msg.Obj;
                        viewModel.HandleReceivedData(readBytes);
                        break;
                    case Constants.MESSAGE_WRITE:
                        byte[] writeBytes = (byte[])msg.Obj;
                        viewModel.HandleSentData(writeBytes);
                        break;
                }
            }
        }

        class BluetoothChatService
        {
            const string TAG = "BluetoothChatService";
            const string NAME_SECURE = "BluetoothChatSecure";
            static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");

            BluetoothAdapter btAdapter;
            Handler handler;
            ConnectThread connectThread;
            ConnectedThread connectedThread;
            int state;
            int newState;

            public const int STATE_NONE = 0;       // we're doing nothing
            public const int STATE_LISTEN = 1;     // now listening for incoming connections
            public const int STATE_CONNECTING = 2; // now initiating an outgoing connection
            public const int STATE_CONNECTED = 3;  // now connected to a remote device

            public BluetoothChatService(Handler handler)
            {
                btAdapter = BluetoothAdapter.DefaultAdapter;
                state = STATE_NONE;
                newState = state;
                this.handler = handler;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            void UpdateUserInterfaceTitle()
            {
                state = GetState();
                newState = state;
                handler.ObtainMessage(Constants.MESSAGE_STATE_CHANGE, newState, -1).SendToTarget();
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public int GetState()
            {
                return state;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Start()
            {
                if (connectThread != null)
                {
                    connectThread.Cancel();
                    connectThread = null;
                }

                if (connectedThread != null)
                {
                    connectedThread.Cancel();
                    connectedThread = null;
                }

                state = STATE_LISTEN;
                UpdateUserInterfaceTitle();
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Connect(BluetoothDevice device)
            {
                if (state == STATE_CONNECTING)
                {
                    if (connectThread != null)
                    {
                        connectThread.Cancel();
                        connectThread = null;
                    }
                }

                // Cancel any thread currently running a connection
                if (connectedThread != null)
                {
                    connectedThread.Cancel();
                    connectedThread = null;
                }

                // Start the thread to connect with the given device
                connectThread = new ConnectThread(device, this);
                connectThread.Start();

                state = STATE_CONNECTING;
                UpdateUserInterfaceTitle();
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Connected(BluetoothSocket socket, BluetoothDevice device, string socketType)
            {
                // Cancel the thread that completed the connection
                if (connectThread != null)
                {
                    connectThread.Cancel();
                    connectThread = null;
                }

                // Cancel any thread currently running a connection
                if (connectedThread != null)
                {
                    connectedThread.Cancel();
                    connectedThread = null;
                }

                // Start the thread to manage the connection and perform transmissions
                connectedThread = new ConnectedThread(socket, this, socketType);
                connectedThread.Start();

                state = STATE_CONNECTED;

                // Send the name of the connected device back to the UI Activity
                var msg = handler.ObtainMessage(Constants.MESSAGE_DEVICE_NAME);
                Bundle bundle = new Bundle();
                bundle.PutString(Constants.DEVICE_NAME, device.Name);
                msg.Data = bundle;
                handler.SendMessage(msg);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void Stop()
            {
                if (connectThread != null)
                {
                    connectThread.Cancel();
                    connectThread = null;
                }

                if (connectedThread != null)
                {
                    connectedThread.Cancel();
                    connectedThread = null;
                }

                state = STATE_NONE;
                UpdateUserInterfaceTitle();
            }

            public void Write(byte[] @out)
            {
                // Create temporary object
                ConnectedThread r;
                // Synchronize a copy of the ConnectedThread
                lock (this)
                {
                    if (state != STATE_CONNECTED)
                    {
                        return;
                    }
                    r = connectedThread;
                }
                // Perform the write unsynchronized
                r.Write(@out);
            }

            void ConnectionFailed()
            {
                state = STATE_LISTEN;

                var msg = handler.ObtainMessage(Constants.MESSAGE_TOAST);
                var bundle = new Bundle();
                bundle.PutString(Constants.TOAST, "Unable to connect device");
                msg.Data = bundle;
                handler.SendMessage(msg);

                Start();
            }

            public void ConnectionLost()
            {
                var msg = handler.ObtainMessage(Constants.MESSAGE_TOAST);
                var bundle = new Bundle();
                bundle.PutString(Constants.TOAST, "Device connection was lost");
                msg.Data = bundle;
                handler.SendMessage(msg);

                state = STATE_NONE;
                UpdateUserInterfaceTitle();
                Start();
            }

            protected class ConnectThread : Thread
            {
                BluetoothSocket socket;
                BluetoothDevice device;
                BluetoothChatService service;
                string socketType;

                public ConnectThread(BluetoothDevice device, BluetoothChatService service)
                {
                    this.device = device;
                    this.service = service;
                    BluetoothSocket tmp = null;

                    try
                    {
                        tmp = device.CreateRfcommSocketToServiceRecord(MY_UUID_SECURE);
                    }
                    catch (Java.IO.IOException e)
                    {
                        Log.Error(TAG, "create() failed", e);
                    }
                    socket = tmp;
                }

                public override void Run()
                {
                    Name = $"ConnectThread_{socketType}";

                    // Always cancel discovery because it will slow down connection
                    service.btAdapter.CancelDiscovery();

                    // Make a connection to the BluetoothSocket
                    try
                    {
                        // This is a blocking call and will only return on a
                        // successful connection or an exception
                        socket.Connect();
                    }
                    catch (Java.IO.IOException e)
                    {
                        // Close the socket
                        try
                        {
                            socket.Close();
                        }
                        catch (Java.IO.IOException e2)
                        {
                            Log.Error(TAG, $"unable to close() {socketType} socket during connection failure.", e2);
                        }

                        // Start the service over to restart listening mode
                        service.ConnectionFailed();
                        return;
                    }

                    // Reset the ConnectThread because we're done
                    lock (this)
                    {
                        service.connectThread = null;
                    }

                    // Start the connected thread
                    service.Connected(socket, device, socketType);
                }

                public void Cancel()
                {
                    try
                    {
                        socket.Close();
                    }
                    catch (Java.IO.IOException e)
                    {
                        Log.Error(TAG, "close() of connect socket failed", e);
                    }
                }
            }

            class ConnectedThread : Thread
            {
                BluetoothSocket socket;
                Stream inStream;
                Stream outStream;
                BluetoothChatService service;

                public ConnectedThread(BluetoothSocket socket, BluetoothChatService service, string socketType)
                {
                    this.socket = socket;
                    this.service = service;
                    Stream tmpIn = null;
                    Stream tmpOut = null;

                    // Get the BluetoothSocket input and output streams
                    try
                    {
                        tmpIn = socket.InputStream;
                        tmpOut = socket.OutputStream;
                    }
                    catch (Java.IO.IOException e)
                    {
                        Log.Error(TAG, "temp sockets not created", e);
                    }

                    inStream = tmpIn;
                    outStream = tmpOut;
                }

                public override void Run()
                {
                    byte[] buffer = new byte[1024];
                    int bytes;

                    // Keep listening to the InputStream while connected
                    while (true)
                    {
                        try
                        {
                            // Read from the InputStream
                            bytes = inStream.Read(buffer, 0, buffer.Length);

                            // Send the obtained bytes to the UI Activity
                            service.handler
                                .ObtainMessage(Constants.MESSAGE_READ, bytes, -1, buffer)
                                .SendToTarget();
                        }
                        catch (Java.IO.IOException e)
                        {
                            Log.Error(TAG, "disconnected", e);
                            service.ConnectionLost();
                            break;
                        }
                    }
                }

                public void Write(byte[] buffer)
                {
                    try
                    {
                        outStream.Write(buffer, 0, buffer.Length);

                        // Share the sent message with the UI Activity
                        service.handler
                            .ObtainMessage(Constants.MESSAGE_WRITE, -1, -1, buffer)
                            .SendToTarget();
                    }
                    catch (Java.IO.IOException e)
                    {
                        Log.Error(TAG, "Exception during write", e);
                    }
                }

                public void Cancel()
                {
                    try
                    {
                        socket.Close();
                    }
                    catch (Java.IO.IOException e)
                    {
                        Log.Error(TAG, "close() of connect socket failed", e);
                    }
                }
            }
        }

        static class Constants
        {
            // Message types sent from the BluetoothChatService Handler
            public const int MESSAGE_STATE_CHANGE = 1;
            public const int MESSAGE_READ = 2;
            public const int MESSAGE_WRITE = 3;
            public const int MESSAGE_DEVICE_NAME = 4;
            public const int MESSAGE_TOAST = 5;

            // Key names received from the BluetoothChatService Handler
            public const string DEVICE_NAME = "device_name";
            public const string TOAST = "toast";
        }
    }
}
