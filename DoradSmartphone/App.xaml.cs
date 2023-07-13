using DoradSmartphone.Data;
using DoradSmartphone.Services.Bluetooth;

namespace DoradSmartphone;

public partial class App : Application
{    
	public static DatabaseConn DatabaseConn { get; private set; }    

    public App(DatabaseConn databaseConn, IBluetoothService bluetoothService)
    {
        InitializeComponent();

        MainPage = new AppShell();
        DatabaseConn = databaseConn;
        bluetoothService.Start();
        bluetoothService.Accept();
    }
}
