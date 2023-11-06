using System.Linq.Expressions;

namespace ICAP_Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(string id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task RemoveAsync(string id);
        Task UpdateAsync(T entity);
    }
}
