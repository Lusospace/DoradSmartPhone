using DoradSmartphone.Data;
using DoradSmartphone.Services.Bluetooth;

namespace DoradSmartphone;

public partial class App : Application
{
	public static DatabaseConn DatabaseConn { get; private set; }

    public static BluetoothService BluetoothService{ get; private set; }

    public App(DatabaseConn databaseConn, BluetoothService bluetoothService)
    {
        InitializeComponent();

        MainPage = new AppShell();
        DatabaseConn = databaseConn;
        BluetoothService = bluetoothService;
        BluetoothService.Accept();
    }
}
