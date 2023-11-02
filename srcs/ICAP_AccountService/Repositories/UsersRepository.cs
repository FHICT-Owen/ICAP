using ICAP_AccountService.Entities;
using MongoDB.Driver;

namespace ICAP_AccountService.Repositories
{
    public class UsersRepository
    {
        private const string CollectionName = "users";
        private readonly IMongoCollection<User> _dbCollection;
        private readonly FilterDefinitionBuilder<User> _filterBuilder = Builders<User>.Filter;

        public UsersRepository()
        {
            var mongoClient = new MongoClient();
            var database = mongoClient.GetDatabase("Account");
            _dbCollection = database.GetCollection<User>(CollectionName);
        }

        public async Task<IReadOnlyCollection<User>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<User?> GetAsync(string id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(string id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }
    }
}
