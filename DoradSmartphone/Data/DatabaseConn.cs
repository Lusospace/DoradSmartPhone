using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using SQLite;

namespace DoradSmartphone.Data
{
    public class DatabaseConn : IRepository
    {
        SQLiteAsyncConnection db;
        //SQLiteConnection db2;
        public DatabaseConn()
        {
        }
        async Task Init()
        {
            if (db is not null)
                return;

            db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            Console.WriteLine(Constants.DatabasePath.ToString());
            
            await db.CreateTableAsync<User>();
            await db.CreateTableAsync<Speed>();            
            await db.CreateTableAsync<Route>();
            await db.CreateTableAsync<Widget>();
            await db.CreateTableAsync<Exercise>();
            await db.CreateTableAsync<WidgetConfiguration>();
            await db.CreateTableAsync<WidgetConfigurationWidget>();
        }

        public async Task<List<Exercise>> GetItemsAsync()
        {
            await Init();
            return await db.Table<Exercise>().ToListAsync();
        }


        public async Task<Exercise> GetItemAsync(int id)
        {
            await Init();
            return await db.Table<Exercise>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            await Init();
            return await db.Table<User>().Where(u => u.Email== email).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItensAsync<T>(T entity) where T : class
        {
            await Init();
            return await db.InsertAsync(entity);            
        }

        public async Task<int> UpdateItemAsync<T>(T entity) where T : class
        {
            await Init();
            return await db.UpdateAsync(entity);
        }

        public async Task<int> DeleteItemAsync<T>(T entity) where T : class
        {
            await Init();
            return await db.DeleteAsync(entity);
        }        
    }
}
