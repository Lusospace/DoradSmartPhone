using Android.Bluetooth;
using Android.OS;
using Android.Util;
using DoradSmartphone.Helpers;
using Java.Util;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DoradSmartphone.Services.Bluetooth
{
    public class BluetoothService
    {
        const string TAG = "Dorad SmartPhone App";
        static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");

        BluetoothAdapter btAdapter;
        Handler handler;
        AcceptThread acceptThread;
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

        void UpdateUserInterfaceTitle()
        {
            state = GetState();
            newState = state;
            handler.ObtainMessage(Constants.MESSAGE_STATE_CHANGE, newState, -1).SendToTarget();
        }

        public int GetState()
        {
            return state;
        }

        public void Start()
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
            UpdateUserInterfaceTitle();
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
            UpdateUserInterfaceTitle();
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
            UpdateUserInterfaceTitle();

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

            // Send the name of the connected device back to the UI Activity
            var msg = handler.ObtainMessage(Constants.MESSAGE_DEVICE_NAME);
            Bundle bundle = new Bundle();
            bundle.PutString(Constants.GLASSES_NAME, device.Name);
            msg.Data = bundle;
            handler.SendMessage(msg);
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
            UpdateUserInterfaceTitle();
        }

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
            private Stream inputStream;
            private Stream outputStream;
            private BluetoothService service;

            public ConnectedThread(BluetoothSocket socket, BluetoothService service)
            {
                this.socket = socket;
                this.service = service;

                Stream tmpIn = null;
                Stream tmpOut = null;

                try
                {
                    tmpIn = socket.InputStream;
                    tmpOut = socket.OutputStream;
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "temp sockets not created", e);
                }

                inputStream = tmpIn;
                outputStream = tmpOut;
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
                            bytes = inputStream.Read(buffer, 0, buffer.Length);
                            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytes);
                            Console.WriteLine(receivedData);
                            service.handler.ObtainMessage(Constants.MESSAGE_READ, bytes, -1, buffer).SendToTarget();
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

            public void Write(byte[] buffer)
            {
                try
                {
                    outputStream.Write(buffer, 0, buffer.Length);
                    service.handler.ObtainMessage(Constants.MESSAGE_WRITE, -1, -1, buffer).SendToTarget();
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
