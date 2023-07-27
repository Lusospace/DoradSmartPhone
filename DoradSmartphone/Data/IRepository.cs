using DoradSmartphone.Models;

namespace DoradSmartphone.Data
{
    public interface IRepository
    {
        Task<int> SaveItensAsync<T>(T entity) where T : class;
        Task<int> UpdateItemAsync<T>(T entity) where T : class;
        Task<int> DeleteItemAsync<T>(T entity) where T : class;
        Task<List<T>> RecoverItensAsync<T>(T entity) where T : class;
        Task DeleteAllItemsAsync<T>();
        Task<List<T>> RecoverExerciseByUserIdAsync<T>(T entity, int userId) where T : class;
        Task<double> RecoverExerciseSpeedByRouteIdAsync(int routeId);
        Task<User> RecoverUserByEmail(string email);
    }
}
