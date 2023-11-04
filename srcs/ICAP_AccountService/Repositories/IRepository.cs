namespace ICAP_AccountService.Repositories
{
    public interface IRepository<T>
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(string id);
        Task RemoveAsync(Guid id);
        Task RemoveAsync(string id);
        Task UpdateAsync(T entity);
    }
}
