using Android.Bluetooth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ToastProject;

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
        
        BluetoothService btService;
        public IToast toast;

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

        public DashboardViewModel(IToast toast)
        {
            Title = "Welcome";
            CommStatus = "Scanning";
            StatusLabel = true;
            DeviceBool = false;
            this.toast = toast;
            //ConnectedDevices();
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
                    //throw new Exception("Glasses device not found.");
                }
                else
                {
                    CommStatus = "Found Device: " + glasses.Name;
                }
                try
                {
                    //btService = new BluetoothService();
                    //btService.Start();
                    //btService.Connect(glasses);
                    try
                    {
                        //btService.Write(ConvertWidgetToJsonAndBytes());
                        //btService.Accept();
                    }
                    catch (Exception ex)
                    {
                        // Handle write or read error
                        CommStatus = "Error sending JSON file over Bluetooth: " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    CommStatus = "Error connecting to the GATT server: " + ex.Message;
                }
            }
            else
            {
                CommStatus = "Bluetooth is disabled.";
            }
        }

        [RelayCommand]
        public void SendSomething()
        {
            try
            {
                btService = new BluetoothService(toast);
                btService.Write(ConvertWidgetToJsonAndBytes());
            }
            catch (Exception ex)
            {                
                CommStatus = "Error sending JSON file over Bluetooth: " + ex.Message;
            }
            
        }

        private byte[] ConvertWidgetToJsonAndBytes()
        {
            System.Random random = new Random();

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
    }
}
