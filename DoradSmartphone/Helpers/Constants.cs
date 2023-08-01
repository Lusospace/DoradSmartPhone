namespace DoradSmartphone.Helpers;

public static class Constants
{
    public const string DatabaseFilename = "DoradSQLite.db3";    

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);



    // Key names received from the BluetoothChatService Handler    
    public const string GLASSES_NAME = "QCOM-BTD";
    //public const string GLASSES_NAME = "Redmi Note 11";
    public const string TOAST = "toast";    

    //Const for Widget and sreen configuration
    public const int XPosition = 264;
    public const int YPosition = 176;
    public const double widgetWidth = 512; // Original width of the widget in pixels
    public const double widgetHeight = 512; // Original height of the widget in pixels
    public const double targetWidgetWidth = 80; // Target width in XAML
    public const double targetWidgetHeight = 80; // Target height in XAML

    //Const for commands
    public const string STARTRUN = "StartRun";
    public const string STARTDEBUG = "StartDebug";
    public const string STOPRUN = "StopRun";
    public const string STOPDEBUG = "StopDebug";        
}
