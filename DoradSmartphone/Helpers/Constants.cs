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

    // Message types sent from the BluetoothChatService Handler
    public const int MESSAGE_STATE_CHANGE = 1;
    public const int MESSAGE_READ = 2;
    public const int MESSAGE_WRITE = 3;
    public const int MESSAGE_DEVICE_NAME = 4;
    public const int MESSAGE_TOAST = 5;

    // Key names received from the BluetoothChatService Handler    
    public const string GLASSES_NAME = "My GATT Server";
    public const string TOAST = "toast";
}
