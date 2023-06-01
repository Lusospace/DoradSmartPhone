using DoradSmartphone.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Diagnostics;
using ToastProject;

namespace DoradSmartphone.Services;

public class BluetoothLEService
{
    public DeviceCandidate NewDeviceCandidateFromHomePage { get; set; } = new();
    public List<DeviceCandidate> DeviceCandidateList { get; private set; }
    public IBluetoothLE BluetoothLE { get; private set; }
    public IAdapter Adapter { get; private set; }
    public IDevice Device { get; set; }
    public IToast toast;

    public BluetoothLEService()
    {
        BluetoothLE = CrossBluetoothLE.Current;
        Adapter = CrossBluetoothLE.Current.Adapter;
        Adapter.ScanTimeout = 4000;

        Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
        Adapter.DeviceConnected += Adapter_DeviceConnected;
        Adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
        Adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;

        BluetoothLE.StateChanged += BluetoothLE_StateChanged;
    }

    public async Task<List<DeviceCandidate>> ScanForDevicesAsync()
    {
        DeviceCandidateList = new List<DeviceCandidate>();

        try
        {
            IReadOnlyList<IDevice> systemDevices = Adapter.GetSystemConnectedOrPairedDevices();
            foreach (var systemDevice in systemDevices)
            {
                DeviceCandidate deviceCandidate = DeviceCandidateList.FirstOrDefault(d => d.Id == systemDevice.Id);
                if (deviceCandidate == null)
                {
                    DeviceCandidateList.Add(new DeviceCandidate
                    {
                        Id = systemDevice.Id,
                        Name = systemDevice.Name,
                    });
                    toast.MakeToast($"Found {systemDevice.State.ToString().ToLower()} device {systemDevice.Name}.");
                }
            }
            await Adapter.StartScanningForDevicesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to scan nearby Bluetooth LE devices: {ex.Message}.");
            await Shell.Current.DisplayAlert($"Unable to scan nearby Bluetooth LE devices", $"{ex.Message}.", "OK");
        }

        return DeviceCandidateList;
    }

    #region DeviceEventArgs
    private async void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
    {
        DeviceCandidate deviceCandidate = DeviceCandidateList.FirstOrDefault(d => d.Id == e.Device.Id);
        if (deviceCandidate == null)
        {
            DeviceCandidateList.Add(new DeviceCandidate
            {
                Id = e.Device.Id,
                Name = e.Device.Name,
            });
            toast.MakeToast($"Found {e.Device.State.ToString().ToLower()} {e.Device.Name}.");
        }
    }

    private void Adapter_DeviceConnectionLost(object sender, DeviceErrorEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                toast.MakeToast($"{e.Device.Name} connection is lost.");
            }
            catch
            {
                toast.MakeToast($"Device connection is lost.");
            }
        });
    }

    private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                toast.MakeToast($"{e.Device.Name} is connected.");
            }
            catch
            {
                toast.MakeToast($"Device is connected.");
            }
        });
    }

    private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                toast.MakeToast($"{e.Device.Name} is disconnected.");
            }
            catch
            {
                toast.MakeToast($"Device is disconnected.");
            }
        });
    }
    #endregion DeviceEventArgs

    #region BluetoothStateChangedArgs
    private void BluetoothLE_StateChanged(object sender, BluetoothStateChangedArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                toast.MakeToast($"Bluetooth state is {e.NewState}.");
            }
            catch
            {
                toast.MakeToast($"Bluetooth state has changed.");
            }
        });
    }
    #endregion BluetoothStateChangedArgs

#if ANDROID
    #region BluetoothPermissionsn
    public async Task<PermissionStatus> CheckBluetoothPermissions()
    {
        PermissionStatus status = PermissionStatus.Unknown;
        try
        {
            status = await Permissions.CheckStatusAsync<BluetoothLEPermissions>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to check Bluetooth LE permissions: {ex.Message}.");
            await Shell.Current.DisplayAlert($"Unable to check Bluetooth LE permissions", $"{ex.Message}.", "OK");
        }
        return status;
    }

    public async Task<PermissionStatus> RequestBluetoothPermissions()
    {
        PermissionStatus status = PermissionStatus.Unknown;
        try
        {
            status = await Permissions.RequestAsync<BluetoothLEPermissions>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to request Bluetooth LE permissions: {ex.Message}.");
            await Shell.Current.DisplayAlert($"Unable to request Bluetooth LE permissions", $"{ex.Message}.", "OK");
        }
        return status;
    }
    #endregion BluetoothPermissions
#endif
}

