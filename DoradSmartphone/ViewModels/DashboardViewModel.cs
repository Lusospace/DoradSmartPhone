using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using Java.Util;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DoradSmartphone.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel, INotifyPropertyChanged
    {
        [ObservableProperty]
        string commStatus;
        [ObservableProperty]
        bool deviceList;
        [ObservableProperty]
        bool statusLabel;
        [ObservableProperty]
        bool deviceBool;        

        BluetoothAdapter btAdapter;
        BluetoothService btService;                
        Handler handler;
        
        bool isConnected = false;

        private ObservableCollection<DeviceCandidate> devices;        
        public ObservableCollection<DeviceCandidate> Devices
        {
            get { return devices; }
            set
            {
                devices = value;
                OnPropertyChanged(nameof(Devices));
                OnPropertyChanged(nameof(HasDevices));
            }
        }

        public bool HasDevices => Devices != null && Devices.ToList().Count > 0;

        public DashboardViewModel()
        {
            Title = "Welcome";
            CommStatus = "Scanning";
            StatusLabel = true;
            DeviceBool = false;
            //StartScanning();            
            btAdapter = BluetoothAdapter.DefaultAdapter;
            handler = new MyHandler(this);
            ConnectedDevices();
        }

        private async void ConnectedDevices()
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter != null && adapter.IsEnabled)
            {
                var pairedDevices = adapter.BondedDevices;
                var glasses = pairedDevices.FirstOrDefault(bd => bd.Name == Constants.GLASSES_NAME);

                if (glasses == null)
                {
                    CommStatus = "No connected devices found.";
                    throw new Exception("Glass device not found.");
                }
                else
                {
                    CommStatus = "Found Device: " + glasses.Name;
                }
                try
                {
                    btService = new BluetoothService(handler);
                    btService.Start();
                    btService.Connect(glasses);     
                    try
                    {
                        btService.Write(SendJsonFileOverBluetooth());
                    }
                    catch (Exception ex)
                    {
                        // Handle write or read error
                        CommStatus = "Error sending JSON file over Bluetooth: " + ex.Message;
                    }
                }
                catch (Exception ex) {
                    CommStatus = "Error connecting to the GATT server: " + ex.Message;
                }
            }
            else
            {
                CommStatus = "Bluetooth is disabled.";
            }
        }

        private byte[] SendJsonFileOverBluetooth()
        {
            System.Random random = new System.Random();

            List<Widget> widgets = GetWidgets();

            foreach (var widget in widgets)
            {
                widget.XPosition = random.Next(0, 100);
                widget.YPosition = random.Next(0, 100);
            }

            string json = System.Text.Json.JsonSerializer.Serialize(widgets);
            string json2 = JsonConvert.SerializeObject(widgets);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            return byteArray;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public List<Widget> GetWidgets() => new List<Widget>
        {
            new Widget {
            Id = 1, Name = "Battery", FileName = "Images/Widgets/battery.png"
            },
            new Widget {
            Id = 2, Name = "Time", FileName = "Images/Widgets/time.png"
            },
            new Widget {
            Id = 3, Name = "Route", FileName = "Images/Widgets/route.png"
            },
        };

        public void HandleDeviceName(string deviceName)
        {
            // Handle the received device name
        }

        public void HandleStateChange(int newState)
        {
            // Handle the state change
        }

        public void HandleToastMessage(string toastMessage)
        {
            // Handle the toast message
        }

        public void HandleReceivedData(byte[] readBytes)
        {
            // Handle the received data
        }

        public void HandleSentData(byte[] writeBytes)
        {
            // Handle the sent data
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
                        string deviceName = msg.Data.GetString(Constants.GLASSES_NAME);
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


        class BluetoothService
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
}
