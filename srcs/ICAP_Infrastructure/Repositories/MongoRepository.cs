using System.Linq.Expressions;
using ICAP_Infrastructure.Entities;
using MongoDB.Driver;

namespace ICAP_Infrastructure.Repositories
{
    public class MongoRepository<T>(IMongoDatabase database, string collectionName) : IRepository<T>
        where T : IEntity
    {
        private readonly IMongoCollection<T> _dbCollection = database.GetCollection<T>(collectionName);
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public virtual async Task<T?> GetAsync(string id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task CreateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(entity.Id)) entity.Id = Guid.NewGuid().ToString();
            await _dbCollection.InsertOneAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }

        public virtual async Task RemoveAsync(string id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }

        public virtual async Task RemoveAsync(Expression<Func<T, bool>> filter)
        {
            await _dbCollection.DeleteManyAsync(filter);
        }
    }
}
