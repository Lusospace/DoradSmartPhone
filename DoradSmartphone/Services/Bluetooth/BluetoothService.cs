using Android.Bluetooth;
using Android.Util;
using DoradSmartphone.Helpers;
using Java.Util;
using System.Text;
using ToastProject;

namespace DoradSmartphone.Services.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {
        public const int STATE_NONE = 0;
        public const int STATE_LISTEN = 1;
        public const int STATE_CONNECTING = 2;
        public const int STATE_CONNECTED = 3;
        const string TAG = "Dorad SmartPhone App";
        static readonly UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");

        BluetoothAdapter btAdapter;
        AcceptThread acceptThread;
        ConnectedThread connectedThread;

        public IToast toast;

        int state;
        int newState;

        public BluetoothService(IToast toast)
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
            state = STATE_NONE;
            newState = state;
            this.toast = toast;
            Start();
        }

        public int GetState()
        {
            return state;
        }

        public void Start()
        {
           if (btAdapter != null && btAdapter.IsEnabled)
            {
                var pairedDevices = btAdapter.BondedDevices;
                var glasses = pairedDevices.FirstOrDefault(bd => bd.Name == Constants.GLASSES_NAME);

                if (glasses == null)
                {
                    Console.Write("No connected devices found.");
                    //toast.MakeToast($"No connected devices found.");
                }
                else
                {
                    Console.Write("Found Device: " + glasses.Name);
                    //toast.MakeToast($"Found Device: " + glasses.Name);
                }
                try
                {
                    if (acceptThread != null)
                    {
                        acceptThread.Cancel();
                        acceptThread = null;
                    }

                    if (connectedThread != null)
                    {
                        connectedThread.Cancel();
                        connectedThread = null;
                    }

                    state = STATE_LISTEN;
                    UpdateBtStatus();

                    Connect(glasses);
                }
                catch (Exception ex)
                {
                    toast.MakeToast($"Error connecting to Dorad Glasses: " + ex.Message);
                }
            }
        }

        public void Accept()
        {
            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }

            acceptThread = new AcceptThread(this);
            acceptThread.Start();

            state = STATE_LISTEN;
            UpdateBtStatus();
        }

        public void Connect(BluetoothDevice device)
        {
            if (state == STATE_CONNECTING)
            {
                if (connectedThread != null)
                {
                    connectedThread.Cancel();
                    connectedThread = null;
                }
            }

            // Cancel any thread currently running a connection
            if (acceptThread != null)
            {
                acceptThread.Cancel();
                acceptThread = null;
            }
            // Start the thread to connect with the given device
            ConnectAsync(device);
        }

        private async Task ConnectAsync(BluetoothDevice device)
        {
            state = STATE_CONNECTING;
            UpdateBtStatus();

            BluetoothSocket socket = null;

            try
            {
                // This is a blocking call and will only return on a
                // successful connection or an exception
                socket = device.CreateRfcommSocketToServiceRecord(MY_UUID_SECURE);
                await socket.ConnectAsync();
            }
            catch (IOException e)
            {
                // Close the socket
                try
                {
                    socket?.Close();
                }
                catch (IOException e2)
                {
                    Log.Error(TAG, $"unable to close() socket during connection failure.", e2);
                }

                // Start the service over to restart listening mode
                ConnectionFailed();
                return;
            }
            Connected(socket, device);
        }

        public void Stop()
        {
            if (acceptThread != null)
            {
                acceptThread.Cancel();
                acceptThread = null;
            }

            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }

            state = STATE_NONE;
            UpdateBtStatus();
        }

        //Call this one to send data over BT
        public void Write(byte[] data)
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
            r.Write(data);
        }

        void UpdateBtStatus()
        {
            state = GetState();
            newState = state;
        }

        void ConnectionFailed()
        {
            state = STATE_LISTEN;

            // Handle connection failure
            Start();
        }

        public void Connected(BluetoothSocket socket, BluetoothDevice device)
        {
            // Cancel the thread that completed the connection
            if (acceptThread != null)
            {
                acceptThread.Cancel();
                acceptThread = null;
            }

            // Cancel any thread currently running a connection
            if (connectedThread != null)
            {
                connectedThread.Cancel();
                connectedThread = null;
            }

            // Start the thread to manage the connection and perform transmissions
            connectedThread = new ConnectedThread(socket, this);
            connectedThread.Start();

            state = STATE_CONNECTED;

            // Handle successful connection
        }

        void ConnectionLost()
        {
            // Handle lost connection
            state = STATE_NONE;
            UpdateBtStatus();
            Start();
        }

        class AcceptThread
        {
            private BluetoothServerSocket serverSocket;
            private BluetoothService service;

            public AcceptThread(BluetoothService service)
            {
                this.service = service;
                BluetoothServerSocket tmp = null;

                try
                {
                    tmp = service.btAdapter.ListenUsingRfcommWithServiceRecord(TAG, MY_UUID_SECURE);
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "listen() failed", e);
                }

                serverSocket = tmp;
            }

            public void Start()
            {
                Task.Run(async () =>
                {
                    BluetoothSocket socket = null;

                    while (service.state != STATE_CONNECTED)
                    {
                        try
                        {
                            socket = await serverSocket.AcceptAsync();
                        }
                        catch (IOException e)
                        {
                            Log.Error(TAG, "accept() failed", e);
                            break;
                        }

                        if (socket != null)
                        {
                            lock (service)
                            {
                                switch (service.state)
                                {
                                    case STATE_LISTEN:
                                    case STATE_CONNECTING:
                                        service.Connected(socket, socket.RemoteDevice);
                                        break;
                                    case STATE_NONE:
                                    case STATE_CONNECTED:
                                        // Either not ready or already connected. Terminate new socket.
                                        try
                                        {
                                            socket.Close();
                                        }
                                        catch (IOException e)
                                        {
                                            Log.Error(TAG, "Could not close unwanted socket", e);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                });
            }

            public void Cancel()
            {
                try
                {
                    serverSocket.Close();
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "close() of server socket failed", e);
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

            public void Start()
            {
                Task.Run(() =>
                {
                    byte[] buffer = new byte[1024];
                    int bytes;

                    while (true)
                    {
                        try
                        {
                            bytes = inStream.Read(buffer, 0, buffer.Length);
                            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytes);
                            Console.WriteLine(receivedData);
                            // Handle received data
                        }
                        catch (IOException e)
                        {
                            Log.Error(TAG, "disconnected", e);
                            service.ConnectionLost();
                            break;
                        }
                    }
                });
            }

            //This one 
            public void Write(byte[] buffer)
            {
                try
                {
                    outStream.Write(buffer, 0, buffer.Length);                    
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
