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
    public const string GLASSES_NAME = "My GATT Server";
    //public const string GLASSES_NAME = "Redmi Note 11";
    public const string TOAST = "toast";
}
