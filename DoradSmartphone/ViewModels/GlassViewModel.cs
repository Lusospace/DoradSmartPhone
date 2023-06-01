using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class GlassViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public IToast toast;
        BluetoothLEService BluetoothLEService;        

        public ObservableCollection<DeviceCandidate> DeviceCandidates { get; } = new();

        public IAsyncRelayCommand GoToHeartRatePageAsyncCommand { get; }
        public IAsyncRelayCommand ScanNearbyDevicesAsyncCommand { get; }
        public IAsyncRelayCommand CheckBluetoothAvailabilityAsyncCommand { get; }

        public GlassViewModel(BluetoothLEService bluetoothLEService, IToast toast)
        {
            Title = $"Scan and select device";

            BluetoothLEService = bluetoothLEService;

            GoToHeartRatePageAsyncCommand = new AsyncRelayCommand<DeviceCandidate>(async (devicecandidate) => await GoToHeartRatePageAsync(devicecandidate));

            ScanNearbyDevicesAsyncCommand = new AsyncRelayCommand(ScanDevicesAsync);
            CheckBluetoothAvailabilityAsyncCommand = new AsyncRelayCommand(CheckBluetoothAvailabilityAsync);
            this.toast = toast;
        }

        async Task GoToHeartRatePageAsync(DeviceCandidate deviceCandidate)
        {
            if (IsScanning)
            {
                toast.MakeToast($"Bluetooth adapter is scanning. Try again.");
                return;
            }

            if (deviceCandidate == null)
            {
                return;
            }

            BluetoothLEService.NewDeviceCandidateFromHomePage = deviceCandidate;

            Title = $"{deviceCandidate.Name}";

            //await Shell.Current.GoToAsync("//HeartRatePage", true);
        }

        async Task ScanDevicesAsync()
        {
            if (IsScanning)
            {
                return;
            }

            if (!BluetoothLEService.BluetoothLE.IsAvailable)
            {
                Debug.WriteLine($"Bluetooth is missing.");
                await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                return;
            }

#if ANDROID
        PermissionStatus permissionStatus = await BluetoothLEService.CheckBluetoothPermissions();
        if (permissionStatus != PermissionStatus.Granted)
        {
            permissionStatus = await BluetoothLEService.RequestBluetoothPermissions();
            if (permissionStatus != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert($"Bluetooth LE permissions", $"Bluetooth LE permissions are not granted.", "OK");
                return;
            }
        }
#elif IOS
#elif WINDOWS
#endif

            try
            {
                if (!BluetoothLEService.BluetoothLE.IsOn)
                {
                    await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                    return;
                }

                IsScanning = true;

                List<DeviceCandidate> deviceCandidates = await BluetoothLEService.ScanForDevicesAsync();

                if (deviceCandidates.Count == 0)
                {
                    toast.MakeToast($"Unable to find nearby devices. Try again.");
                }

                if (DeviceCandidates.Count > 0)
                {
                    DeviceCandidates.Clear();
                }

                foreach (var deviceCandidate in deviceCandidates)
                {
                    DeviceCandidates.Add(deviceCandidate);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get nearby devices: {ex.Message}");
                 await Shell.Current.DisplayAlert($"Unable to get nearby devices", $"{ex.Message}.", "OK");
            }
            finally
            {
                IsScanning = false;
            }
        }

        async Task CheckBluetoothAvailabilityAsync()
        {
            if (IsScanning)
            {
                return;
            }

            try
            {
                if (!BluetoothLEService.BluetoothLE.IsAvailable)
                {
                    Debug.WriteLine($"Error: Bluetooth is missing.");
                    await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                    return;
                }

                if (BluetoothLEService.BluetoothLE.IsOn)
                {
                    await Shell.Current.DisplayAlert($"Bluetooth is on", $"We're good to go.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to check Bluetooth availability: {ex.Message}");
                await Shell.Current.DisplayAlert($"Unable to check Bluetooth availability", $"{ex.Message}.", "OK");
            }
        }
    }
}
