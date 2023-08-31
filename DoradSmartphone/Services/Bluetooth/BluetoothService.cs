using Android.Bluetooth;
using Android.Util;
using DoradSmartphone.Helpers;
using Java.Util;
using System.Text;

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

        public event EventHandler<string> DataReceived;

        BluetoothAdapter btAdapter;
        ListenerConfiguration listenerConfiguration;
        BluetoothHandlers btHandlers;

        int state;
        int newState;

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            private set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    ConnectionStatusChanged?.Invoke(this, value);
                }
            }
        }

        public event EventHandler<bool> ConnectionStatusChanged;


        public BluetoothService()
        {
            btAdapter = BluetoothAdapter.DefaultAdapter;
            state = STATE_NONE;
            newState = state;
            //Start();
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
                }
                else
                {
                    Console.Write("Found Device: " + glasses.Name);
                }
                try
                {
                    state = STATE_LISTEN;
                    UpdateBtStatus();

                    Connect(glasses);
                    Accept();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to Dorad Glasses: " + ex.Message);
                }
            }
        }

        public void Accept()
        {
            listenerConfiguration = new ListenerConfiguration(this);
            listenerConfiguration.Start();

            state = STATE_LISTEN;
            UpdateBtStatus();
        }

        private void Connect(BluetoothDevice device)
        {
            state = STATE_CONNECTING;
            UpdateBtStatus();

            BluetoothSocket socket = null;

            try
            {
                // This is a blocking call and will only return on a
                // successful connection or an exception
                socket = device.CreateRfcommSocketToServiceRecord(MY_UUID_SECURE);
                socket.ConnectAsync();
                state = STATE_CONNECTED;
                UpdateBtStatus();
                if (state != STATE_CONNECTED)
                {
                    ConnectionFailed();
                }
                else
                {
                    HandleConnection(socket);
                }
            }
            catch (Exception e)
            {
                // Close the socket
                try
                {
                    Log.Error(TAG, $"unable to connect.", e);
                    socket?.Close();
                }
                catch (Exception e2)
                {
                    Log.Error(TAG, $"unable to close() socket during connection failure.", e2);
                }

                // Start the service over to restart listening mode
                ConnectionFailed();
                return;
            }
        }

        /// <summary>
        /// Method responsible to receive the data, verify the connection status and then send it
        /// </summary>
        /// <param name="data"></param>
        public void Write(byte[] data)
        {
            // Synchronize a copy of the BluetoothHandlers
            lock (this)
            {
                if (state != STATE_CONNECTED)
                {
                    Toaster.MakeToast($"No device connected to send.");
                    ConnectionFailed();
                    return;
                }
                else
                {
                    btHandlers.Write(data);
                }
            }
        }

        public void UpdateBtStatus()
        {
            state = GetState();
            newState = state;
            CheckConnection();
        }

        public void ConnectionFailed()
        {
            state = STATE_LISTEN;

            // Handle connection failure
            Start();
        }

        public void HandleConnection(BluetoothSocket socket)
        {
            // Start the thread to manage the connection and perform transmissions
            btHandlers = new BluetoothHandlers(socket, this);
            btHandlers.Start();

            state = STATE_CONNECTED;
        }

        public void ConnectionLost()
        {
            // Handle lost connection            
            state = STATE_NONE;
            UpdateBtStatus();
            Start();
        }

        /// <summary>
        /// Verify the connection status and return it
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CheckConnection()
        {
            try
            {
                int connectionState = GetState();

                IsConnected = connectionState == STATE_CONNECTED;

                return IsConnected;
            }
            catch (Exception ex)
            {
                Toaster.MakeToast("Error " + ex);
                throw new Exception("Error when checking the Bluetooth status.");
            }
        }

        /// <summary>
        /// This Class can't be separated because some BT classes can't be instaciated due the protection level. 
        /// To modularize it, will be necessary to refactor the entire service.
        /// </summary>
        class ListenerConfiguration
        {
            private BluetoothServerSocket serverSocket;
            private BluetoothService service;

            public ListenerConfiguration(BluetoothService service)
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
                                        service.HandleConnection(socket);
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

        /// <summary>
        /// This Class can't be separated because some BT classes can't be instaciated due the protection level. 
        /// To modularize it, will be necessary to refactor the entire service.
        /// </summary>
        class BluetoothHandlers
        {
            private static readonly string TAG = "BluetoothHandlers";
            private BluetoothSocket socket;
            private Stream inStream;
            private Stream outStream;
            private BluetoothService service;

            public BluetoothHandlers(BluetoothSocket socket, BluetoothService service)
            {
                Log.Debug(TAG, "create BluetoothHandlers");

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
                    byte[] buffer = new byte[2048];
                    int bytes;

                    while (true)
                    {
                        try
                        {
                            bytes = inStream.Read(buffer, 0, buffer.Length);
                            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytes);                            
                            MessageHandler.ProcessReceivedData(receivedData);
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

            //This one do the send
            public void Write(byte[] buffer)
            {
                try
                {
                    outStream.Write(buffer);
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Exception during write", e);
                }
            }
        }

        /// <summary>
        /// Event Listener for the Received messages from bluetooth
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnDataReceived(string data)
        {
            DataReceived?.Invoke(this, data); // Invoke the event
        }
    }
}
