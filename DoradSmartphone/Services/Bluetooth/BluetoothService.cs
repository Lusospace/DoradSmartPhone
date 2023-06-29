using Android.Bluetooth;
using Android.OS;
using Android.Util;
using DoradSmartphone.Helpers;
using Java.Util;
using System.Runtime.CompilerServices;

namespace DoradSmartphone.Services.Bluetooth
{
    public class BluetoothService
    {
        const string TAG = "Dorad SmartPhone App";        
        static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");

        BluetoothAdapter btAdapter;
        Handler handler;
        ConnectThread connectThread;
        ConnectedThread connectedThread;
        int state;
        int newState;

        public const int STATE_NONE = 0;
        public const int STATE_LISTEN = 1;
        public const int STATE_CONNECTING = 2;
        public const int STATE_CONNECTED = 3; 

        public BluetoothService(Handler handler)
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
            _ = connectThread.Run();

            state = STATE_CONNECTING;
            UpdateUserInterfaceTitle();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Connected(BluetoothSocket socket, BluetoothDevice device)
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
            connectedThread = new ConnectedThread(socket, this);
            _ = connectedThread.Run();

            state = STATE_CONNECTED;

            // Send the name of the connected device back to the UI Activity
            var msg = handler.ObtainMessage(Constants.MESSAGE_DEVICE_NAME);
            Bundle bundle = new Bundle();
            bundle.PutString(Constants.GLASSES_NAME, device.Name);
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

        class ConnectThread
        {
            private BluetoothSocket socket;
            private BluetoothDevice device;
            private BluetoothService service;

            public ConnectThread(BluetoothDevice device, BluetoothService service)
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

            public async Task Run()
            {
                // Always cancel discovery because it will slow down connection
                service.btAdapter.CancelDiscovery();

                // Make a connection to the BluetoothSocket
                try
                {
                    // This is a blocking call and will only return on a
                    // successful connection or an exception
                    await socket.ConnectAsync();
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
                        Log.Error(TAG, $"unable to close() socket during connection failure.", e2);
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
                service.Connected(socket, device);
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

        class ConnectedThread
        {
            private static readonly string TAG = "ConnectedThread";
            private BluetoothSocket socket;
            private Stream inStream;
            private Stream outStream;
            private BluetoothService service;

            public ConnectedThread(BluetoothSocket socket, BluetoothService service)
            {
                Log.Debug(TAG, "create ConnectedThread");

                this.socket = socket;
                this.service = service;

                try
                {
                    inStream = socket.InputStream;
                    outStream = socket.OutputStream;
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Failed to create IO streams", e);
                }
            }

            public async Task Run()
            {
                Log.Info(TAG, "BEGIN mConnectedThread");

                byte[] buffer = new byte[1024];
                int bytes;

                while (true)
                {
                    try
                    {
                        bytes = await inStream.ReadAsync(buffer, 0, buffer.Length);

                        if (bytes > 0)
                        {
                            // Send the obtained bytes to the UI Activity
                            service.handler
                                .ObtainMessage(Constants.MESSAGE_READ, bytes, -1, buffer)
                                .SendToTarget();
                        }
                    }
                    catch (IOException e)
                    {
                        Log.Error(TAG, "Disconnected", e);
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

                    // Share the sent message back to the UI Activity
                    service.handler
                        .ObtainMessage(Constants.MESSAGE_WRITE, -1, -1, buffer)
                        .SendToTarget();
                }
                catch (IOException e)
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
                catch (IOException e)
                {
                    Log.Error(TAG, "close() of connect socket failed", e);
                }
            }
        }
    }
}
