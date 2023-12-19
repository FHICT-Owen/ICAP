using ICAP_Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ICAP_Infrastructure.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>() ?? throw new ArgumentNullException(nameof(IConfiguration));
                var settings = MongoClientSettings.FromConnectionString(configuration["MongoConnectionString"]);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var mongoClient = new MongoClient(settings);
                return mongoClient.GetDatabase(configuration["MongoDatabaseName"]);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
            where T : IEntity
        {
            services.AddTransient<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database ?? throw new InvalidOperationException(), collectionName);
            });
            return services;
        }
    }
}
