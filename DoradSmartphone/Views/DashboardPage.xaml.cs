using Android.Bluetooth;
using Android.Content;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using Newtonsoft.Json;

namespace DoradSmartphone.Views;

public partial class DashboardPage : ContentPage
{
    private DashboardViewModel viewModel;

    public DashboardPage()
    {
        InitializeComponent();
        viewModel = new DashboardViewModel();
        BindingContext = viewModel;
        StartScanning();
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
                receiver.Devices.Add(new DeviceCandidate { Name = device.Name });
            }

            adapter.StartDiscovery();
            await Task.Delay(5000); // Scan for 5 seconds (adjust as needed)

            adapter.CancelDiscovery();
            MauiApplication.Current.ApplicationContext.UnregisterReceiver(receiver);

            var devices = receiver.Devices;
            if (devices.Count > 0)
            {
                viewModel.Devices = devices;
                DeviceListView.IsVisible = true;
                StatusLabel.IsVisible = false;
            }
            else
            {
                StatusLabel.Text = "No devices found.";
            }
        }
        else
        {
            StatusLabel.Text = "Bluetooth is disabled.";
        }
    }

    private void SendJsonFile()
    {
        var json = JsonConvert.SerializeObject(viewModel.Devices);
        // Send the JSON file over Bluetooth or perform any other desired actions
    }
}