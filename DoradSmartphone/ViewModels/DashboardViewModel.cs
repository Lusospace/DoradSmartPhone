using Android.Bluetooth;
using Android.Content;
using AndroidX.Lifecycle;
using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<DeviceCandidate> devices;
        public List<DeviceCandidate> Devices
        {
            get { return devices; }
            set
            {
                devices = value;
                OnPropertyChanged(nameof(Devices));
                OnPropertyChanged(nameof(HasDevices));
            }
        }

        public bool HasDevices => Devices != null && Devices.Count > 0;


        public DashboardViewModel() { 
            Title = "Welcome";
            CommStatus = "Scaning";
            StatusLabel = true;
            DeviceBool = false;
            //StartScanning();
        }

        private async void StartScanning()
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter != null && adapter.IsEnabled)
            {
                var receiver = new BluetoothReceiver();
                var filter = new IntentFilter(BluetoothDevice.ActionFound);
                MauiApplication.Current.ApplicationContext.RegisterReceiver(receiver, filter);

                // Retrieve already paired devices
                var pairedDevices = adapter.BondedDevices;
                foreach (var device in pairedDevices)
                {
                    receiver.Devices.Add(new DeviceCandidate {
                        Name = device.Name,
                        Address = device.Address
                    });
                }

                adapter.StartDiscovery();
                await Task.Delay(5000); // Scan for 5 seconds (adjust as needed)

                adapter.CancelDiscovery();
                MauiApplication.Current.ApplicationContext.UnregisterReceiver(receiver);

                var devices = receiver.Devices;
                if (devices.Count > 0)
                {
                    Devices = devices;
                    DeviceBool = true;
                    //HasDevices = true;
                    StatusLabel = false;
                }
                else
                {
                    CommStatus = "No devices found.";
                }
            }
            else
            {
                CommStatus = "Bluetooth is disabled.";
            }
        }

        private void SendJsonFile()
        {
            var json = JsonConvert.SerializeObject(Devices);
            //if (chatService.GetState() != BluetoothChatService.STATE_CONNECTED)
            //{
            //    Toast.MakeText(Activity, Resource.String.not_connected, ToastLength.Long).Show();
            //    return;
            //}

            //if (message.Length > 0)
            //{
            //    var bytes = Encoding.ASCII.GetBytes(message);
            //    chatService.Write(bytes);
            //    outStringBuffer.Clear();
            //    outEditText.Text = outStringBuffer.ToString();
            //}
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
