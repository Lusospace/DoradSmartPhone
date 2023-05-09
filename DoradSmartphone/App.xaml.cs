using DoradSmartphone.Data;

namespace DoradSmartphone;

public partial class App : Application
{
	public static DatabaseConn DatabaseConn { get; private set; }
	public App(DatabaseConn databaseConn)
	{
		InitializeComponent();

		MainPage = new AppShell();
        DatabaseConn = databaseConn;
	}
}
