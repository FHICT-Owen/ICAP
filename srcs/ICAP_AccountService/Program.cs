using ICAP_AccountService.Entities;
using ICAP_Infrastructure;
using ICAP_Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Reflection;

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
} catch (Exception) { }


builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedSpecificOrigins", policy =>
        policy.WithOrigins("http://localhost",
                            "https://localhost",
                            "https://icap.odb-tech.com")
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    if (!builder.Environment.IsDevelopment()) c.DocumentFilter<BasePathFilter>("/accounts");
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

builder.Services.AddMongo()
    .AddMongoRepository<User>("users");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowedSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
