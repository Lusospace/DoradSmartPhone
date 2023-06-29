using Android.Bluetooth;
using Android.OS;
using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                    throw new Exception("Glasses device not found.");
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
                        btService.Write(ConvertWidgetToJsonAndBytes());
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

        private byte[] ConvertWidgetToJsonAndBytes()
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
    }
}
