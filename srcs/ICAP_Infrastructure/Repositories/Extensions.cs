using ICAP_Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                var environment = serviceProvider.GetService<IWebHostEnvironment>() ?? throw new ArgumentNullException(nameof(IWebHostEnvironment));
                var settings = environment.IsProduction()
                    ? MongoClientSettings.FromConnectionString(configuration["MongoConnectionString"])
                    : MongoClientSettings.FromConnectionString(configuration["MongoTestConnectionString"]);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var mongoClient = new MongoClient(settings);
                return environment.IsProduction()
                    ? mongoClient.GetDatabase(configuration["MongoDatabaseName"])
                    : mongoClient.GetDatabase(configuration["MongoTestDatabaseName"]);
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
