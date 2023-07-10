namespace DoradSmartphone.Data
{
    public interface IRepository
    {
        Task<int> SaveItensAsync<T>(T entity) where T : class;
        Task<int> UpdateItemAsync<T>(T entity) where T : class;
        Task<int> DeleteItemAsync<T>(T entity) where T : class;        
        Task<List<T>> RecoverItensAsync<T>(T entity) where T : class;
        Task DeleteAllItemsAsync<T>();
    }
}