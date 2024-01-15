using ICAP_AccountService.Entities;
using ICAP_AccountService.Events;
using ICAP_Infrastructure.Repositories;
using ICAP_ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

const string secretPath = "/mnt/secrets-store";
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

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UseAzureServiceBusPublisher(builder.Configuration);
builder.Services.UseAzureServiceBusHandler(builder.Configuration);

builder.Services.AddMongo()
    .AddMongoRepository<User>("users");

builder.Services.AddSingleton<FriendRequestAccepted>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
