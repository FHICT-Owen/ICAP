using ICAP_Infrastructure;
using ICAP_Infrastructure.Repositories;
using ICAP_MarketService.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Reflection;
using ICAP_MarketService.Collections;

var builder = WebApplication.CreateBuilder(args);

const string secretPath = "/mnt/secrets-store";
try
{
    var secretFiles = Directory.GetFiles(secretPath);
    if (secretFiles.Any())
    {
        foreach (var file in secretFiles)
        {
            var secretName = Path.GetFileName(file);
            var secretValue = File.ReadAllText(file);
            Environment.SetEnvironmentVariable(secretName, secretValue);
        }
    }
}
catch (Exception) { }

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DynamicOriginPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                var uri = new Uri(origin);
                var allowedHosts = new List<string>
                {
                    "localhost",
                    "icap.odb-tech.com"
                };
                return allowedHosts.Any(allowedHost => uri.Host.Equals(allowedHost, StringComparison.OrdinalIgnoreCase));
            })
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    if (!builder.Environment.IsDevelopment()) c.DocumentFilter<BasePathFilter>("/market");
});

builder.Services.AddMassTransit(x =>
{
    var entryAssembly = Assembly.GetEntryAssembly();
    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);

    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host(builder.Configuration["AzureServiceBus"]);
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<MarketListingCollection>();
builder.Services.AddMongo()
    .AddMongoRepository<MarketListing>("marketlistings");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("DynamicOriginPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
