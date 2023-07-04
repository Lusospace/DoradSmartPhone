using System.IO;
using System.Runtime.CompilerServices;
using Android.Bluetooth;
using Android.OS;
using Android.Util;
using Java.Lang;
using Java.Util;

namespace com.xamarin.samples.bluetooth.bluetoothchat
{

    class BluetoothChatService
    {
        const string TAG = "BluetoothChatService";

        const string NAME_SECURE = "BluetoothChatSecure";

        static UUID MY_UUID_SECURE = UUID.FromString("fa87c0d0-afac-11de-8a39-0800200c9a66");

        BluetoothAdapter btAdapter;
        Handler handler;
        AcceptThread secureAcceptThread;
        ConnectThread connectThread;
        ConnectedThread connectedThread;
        int state;
        int newState;

        public const int STATE_NONE = 0;       // we're doing nothing
        public const int STATE_LISTEN = 1;     // now listening for incoming connections
        public const int STATE_CONNECTING = 2; // now initiating an outgoing connection
        public const int STATE_CONNECTED = 3;  // now connected to a remote device

        public BluetoothChatService( Handler handler)
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

            if (secureAcceptThread == null)
            {
                secureAcceptThread = new AcceptThread(this);
                secureAcceptThread.Start();
            }
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


            if (secureAcceptThread != null)
            {
                secureAcceptThread.Cancel();
                secureAcceptThread = null;
            }

            // Start the thread to manage the connection and perform transmissions
            connectedThread = new ConnectedThread(socket, this, socketType);
            connectedThread.Start();

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

            if (secureAcceptThread != null)
            {
                secureAcceptThread.Cancel();
                secureAcceptThread = null;
            }

            state = STATE_NONE;
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
            bundle.PutString(Constants.TOAST, "Unable to connect device.");
            msg.Data = bundle;
            handler.SendMessage(msg);

            state = STATE_NONE;
            UpdateUserInterfaceTitle();
            this.Start();
        }
        class AcceptThread : Thread
        {
            // The local server socket
            BluetoothServerSocket serverSocket;
            string socketType;
            BluetoothChatService service;

            public AcceptThread(BluetoothChatService service)
            {
                BluetoothServerSocket tmp = null;                
                this.service = service;

                try
                {
                   tmp = service.btAdapter.ListenUsingRfcommWithServiceRecord(NAME_SECURE, MY_UUID_SECURE);
                }
                catch (Java.IO.IOException e)
                {
                    Log.Error(TAG, "listen() failed", e);
                }
                serverSocket = tmp;
                service.state = STATE_LISTEN;
            }

            public override void Run()
            {
                Name = $"AcceptThread_{socketType}";
                BluetoothSocket socket = null;

                while (service.GetState() != STATE_CONNECTED)
                {
                    try
                    {
                        socket = serverSocket.Accept();
                    }
                    catch (Java.IO.IOException e)
                    {
                        Log.Error(TAG, "accept() failed", e);
                        break;
                    }

                    if (socket != null)
                    {
                        lock (this)
                        {
                            switch (service.GetState())
                            {
                                case STATE_LISTEN:
                                case STATE_CONNECTING:
                                    // Situation normal. Start the connected thread.
                                    service.Connected(socket, socket.RemoteDevice, socketType);
                                    break;
                                case STATE_NONE:
                                case STATE_CONNECTED:
                                    try
                                    {
                                        socket.Close();
                                    }
                                    catch (Java.IO.IOException e)
                                    {
                                        Log.Error(TAG, "Could not close unwanted socket", e);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            public void Cancel()
            {
                try
                {
                    serverSocket.Close();
                }
                catch (Java.IO.IOException e)
                {
                    Log.Error(TAG, "close() of server failed", e);
                }
            }
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
                service.state = STATE_CONNECTING;
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
                Log.Debug(TAG, $"create ConnectedThread: {socketType}");
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
                service.state = STATE_CONNECTED;
            }

            public override void Run()
            {
                Log.Info(TAG, "BEGIN mConnectedThread");
                byte[] buffer = new byte[1024];
                int bytes;

                // Keep listening to the InputStream while connected
                while (service.GetState() == STATE_CONNECTED)
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

                    // Share the sent message back to the UI Activity
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
}