using Android.App;
using Android.Bluetooth;
using Android.OS;
using Android.Widget;

namespace DoradSmartphone.Services.Bluetooth
{
    public class BluetoothService : Fragment
    {

        const int REQUEST_CONNECT_DEVICE_SECURE = 1;
        const int REQUEST_CONNECT_DEVICE_INSECURE = 2;
        const int REQUEST_ENABLE_BT = 3;

        BluetoothAdapter bluetoothAdapter = null;
        DiscoverableModeReceiverService receiver;
        //ChatHandler handler;
        //WriteListener writeListener;

        bool requestingPermissionsSecure, requestingPermissionsInsecure;

        public BluetoothService()
        {
            StartScanning();
        }

        public void StartScanning()
        {            
            bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            receiver = new DiscoverableModeReceiverService();
            receiver.BluetoothDiscoveryModeChanged += (sender, e) =>
            {
                Console.WriteLine("something");
            };

            if (bluetoothAdapter == null)
            {
                Toast.MakeText(Activity, "Bluetooth is not available.", ToastLength.Long).Show();
                Activity.FinishAndRemoveTask();
            }
            Console.WriteLine(bluetoothAdapter);

            var x = bluetoothAdapter.StartDiscovery();

            Task.Delay(5000);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            
        }

        //public override void OnStart()
        //{
        //    base.OnStart();
        //    if (!bluetoothAdapter.IsEnabled)
        //    {
        //        var enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
        //        StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
        //    }
        //    else if (chatService == null)
        //    {
        //        SetupChat();
        //    }

        //    // Register for when the scan mode changes
        //    var filter = new IntentFilter(BluetoothAdapter.ActionScanModeChanged);
        //    Activity.RegisterReceiver(receiver, filter);
        //}

        //public override void OnResume()
        //{
        //    base.OnResume();
        //    if (chatService != null)
        //    {
        //        if (chatService.GetState() == BluetoothChatService.STATE_NONE)
        //        {
        //            chatService.Start();
        //        }
        //    }
        //}

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        //{
        //    var allGranted = grantResults.AllPermissionsGranted();
        //    if (requestCode == PermissionUtils.RC_LOCATION_PERMISSIONS)
        //    {
        //        if (requestingPermissionsSecure)
        //        {
        //            PairWithBlueToothDevice(true);
        //        }
        //        if (requestingPermissionsInsecure)
        //        {
        //            PairWithBlueToothDevice(false);
        //        }

        //        requestingPermissionsSecure = false;
        //        requestingPermissionsInsecure = false;
        //    }
        //}

        //public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    switch (requestCode)
        //    {
        //        case REQUEST_CONNECT_DEVICE_SECURE:
        //            if (Result.Ok == resultCode)
        //            {
        //                ConnectDevice(data, true);
        //            }
        //            break;
        //        case REQUEST_CONNECT_DEVICE_INSECURE:
        //            if (Result.Ok == resultCode)
        //            {
        //                ConnectDevice(data, true);
        //            }
        //            break;
        //        case REQUEST_ENABLE_BT:
        //            if (Result.Ok == resultCode)
        //            {
        //                Toast.MakeText(Activity, Resource.String.bt_not_enabled_leaving, ToastLength.Short).Show();
        //                Activity.FinishAndRemoveTask();
        //            }
        //            break;
        //    }
        //}

        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    Activity.UnregisterReceiver(receiver);
        //    if (chatService != null)
        //    {
        //        chatService.Stop();
        //    }
        //}

        //void PairWithBlueToothDevice(bool secure)
        //{
        //    requestingPermissionsSecure = false;
        //    requestingPermissionsInsecure = false;

        //    // Bluetooth is automatically granted by Android. Location, OTOH,
        //    // is considered a "dangerous permission" and as such has to 
        //    // be explicitly granted by the user.
        //    if (!Activity.HasLocationPermissions())
        //    {
        //        requestingPermissionsSecure = secure;
        //        requestingPermissionsInsecure = !secure;
        //        this.RequestPermissionsForApp();
        //        return;
        //    }

        //    var intent = new Intent(Activity, typeof(DeviceListActivity));
        //    if (secure)
        //    {
        //        StartActivityForResult(intent, REQUEST_CONNECT_DEVICE_SECURE);
        //    }
        //    else
        //    {
        //        StartActivityForResult(intent, REQUEST_CONNECT_DEVICE_INSECURE);
        //    }
        //}
    }
}
