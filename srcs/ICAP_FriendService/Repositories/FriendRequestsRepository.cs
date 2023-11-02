using ICAP_FriendService.Entities;
using MongoDB.Driver;

namespace ICAP_FriendService.Repositories
{
    public class FriendRequestsRepository
    {
        private const string CollectionName = "requests";
        private readonly IMongoCollection<FriendRequest> _dbCollection;
        private readonly FilterDefinitionBuilder<FriendRequest> _filterBuilder = Builders<FriendRequest>.Filter;

        public FriendRequestsRepository()
        {
            var mongoClient = new MongoClient();
            var database = mongoClient.GetDatabase("Friendships");
            _dbCollection = database.GetCollection<FriendRequest>(CollectionName);
        }

        public async Task<IReadOnlyCollection<FriendRequest>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<FriendRequest?> GetAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(entity => entity.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(FriendRequest entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(FriendRequest entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }
    }
}
