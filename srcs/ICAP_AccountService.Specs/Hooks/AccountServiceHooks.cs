using DotNetEnv;
using ICAP_AccountService.Entities;
using ICAP_Infrastructure.Repositories;

namespace ICAP_AccountService.Specs.Hooks
{
    using BoDi;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    namespace ICAP_AccountService.Specs.Hooks
    {
        [Binding]
        public class AccountServiceHooks(IObjectContainer objectContainer)
        {
            [BeforeScenario]
            public async Task RegisterServices()
            {
                var factory = GetWebApplicationFactory();
                await ClearData(factory);
                objectContainer.RegisterInstanceAs(factory);
                var repository = (IRepository<User>)factory.Services.GetService(typeof(IRepository<User>))!;
                objectContainer.RegisterInstanceAs(repository);
            }

            private WebApplicationFactory<Program> GetWebApplicationFactory() =>
                new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureAppConfiguration((_, config) =>
                        {
                            config.AddUserSecrets<Program>();
                            config.AddEnvironmentVariables();
                            Env.TraversePath().Load();
                        });
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddMongo()
                                .AddMongoRepository<User>("users");
                        });
                    });

            private async Task ClearData(
                WebApplicationFactory<Program> factory)
            {
                if (factory.Services.GetService(typeof(IRepository<User>))
                    is not IRepository<User> repository) return;
                var entities = await repository.GetAllAsync();
                foreach (var entity in entities)
                    await repository.RemoveAsync(entity.Id);
            }
        }
    }
}
