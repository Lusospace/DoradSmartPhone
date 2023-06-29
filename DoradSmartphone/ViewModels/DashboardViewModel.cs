using Android.Bluetooth;
using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

        private BluetoothSocket _socket;

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
            ConnectedDevices();
        }

        private async void ConnectedDevices()
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter != null && adapter.IsEnabled)
            {
                var pairedDevices = adapter.BondedDevices;
                var glasses = pairedDevices.FirstOrDefault(bd => bd.Name == "My GATT Server");

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

                
                _socket = glasses.CreateRfcommSocketToServiceRecord(glasses.GetUuids().FirstOrDefault().Uuid);

                await _socket.ConnectAsync();

                // Read data from the device
                //await _socket.InputStream.ReadAsync(buffer, 0, buffer.Length);

                System.Random random = new System.Random();

                List<Widget> widgets = GetWidgets();

                foreach (var widget in widgets)
                {
                    widget.XPosition = random.Next(0, 100);
                    widget.YPosition = random.Next(0, 100);
                }

                string json = System.Text.Json.JsonSerializer.Serialize(widgets);
                string json2 = JsonConvert.SerializeObject(widgets);
                //string json = JsonConvert.SerializeObject(widgets, Formatting.Indented);                

                // Write data to the device
                await SendJsonFileOverBluetooth(json);
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

        private async Task SendJsonFileOverBluetooth(string json)
        {
            // Read the JSON file contents into a byte array
            byte[] jsonBytes = File.ReadAllBytes(json);

            try
            {
                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(json);
                await _socket.OutputStream.WriteAsync(dataBytes, 0, dataBytes.Length);
            }
            catch (Exception ex)
            {
                // Handle write or read error
                CommStatus = "Error sending JSON file over Bluetooth: " + ex.Message;
            }
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
    }    
}
