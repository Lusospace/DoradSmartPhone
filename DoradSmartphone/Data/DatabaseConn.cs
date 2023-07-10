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
            await db.CreateTableAsync<WidgetPersonalization>();
        }

        public async Task<Exercise> GetItemAsync(int id)
        {
            await Init();
            return await db.Table<Exercise>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            await Init();
            return await db.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItensAsync<T>(T entity) where T : class
        {
            await Init();

            if (entity is Exercise exercise)
            {
                await db.InsertAsync(entity); // Save the main entity to get the generated primary key ID

                // Retrieve the primary key ID of the inserted exercise entity
                var exerciseId = exercise.Id;

                // Save the speed associated with the exercise
                exercise.Speed.ExerciseId = exerciseId;
                await db.InsertAsync(exercise.Speed);

                // Save the routes associated with the exercise
                foreach (var route in exercise.Route)
                {
                    route.ExerciseId = exerciseId; // Set the reference
                    await db.InsertAsync(route);
                }
            }
            else
            {
                await db.InsertAsync(entity); // Save other entities without setting references
            }

            return 1; // Return a value indicating success
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

        public async Task<List<T>> RecoverItensAsync<T>(T entity) where T : class
        {
            await Init();

            if (typeof(T) == typeof(Exercise))
            {
                var exercises = await db.Table<Exercise>().ToListAsync();
                var usr = await db.Table<Speed>().ToListAsync();

                foreach (var exercise in exercises)
                {
                    exercise.Speed = await db.Table<Speed>().Where(s => s.ExerciseId == exercise.Id).FirstOrDefaultAsync();
                    exercise.Route = await db.Table<Route>().Where(r => r.ExerciseId == exercise.Id).ToListAsync();
                }

                return exercises as List<T>;
            }
            else if (typeof(T) == typeof(User))
            {
                return await db.Table<User>().ToListAsync() as List<T>;
            }
            else if (typeof(T) == typeof(Route))
            {
                return await db.Table<Route>().ToListAsync() as List<T>;
            }

            // Handle other entities or return an empty list if not implemented
            return new List<T>();
        }

        public async Task DeleteAllItemsAsync<T>()
        {
            await Init();
            await db.DeleteAllAsync<T>();
        }
    }
}
