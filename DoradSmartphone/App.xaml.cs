using DoradSmartphone.Data;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;

namespace DoradSmartphone;

public partial class App : Application
{
    public static DatabaseConn DatabaseConn { get; private set; }
    public static BluetoothService BluetoothService { get; private set; }

    public App(DatabaseConn databaseConn, BluetoothService bluetoothService)
    {
        InitializeComponent();

        MainPage = new AppShell();
        DatabaseConn = databaseConn;

        // Register the BluetoothService using the custom service locator
        if (!ServiceLocator.IsRegistered<IBluetoothService>())
        {
            ServiceLocator.Register<IBluetoothService>(bluetoothService);
        }
    }
}