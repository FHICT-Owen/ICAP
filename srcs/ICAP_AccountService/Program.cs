using Azure.Identity;
using ICAP_AccountService.Events;
using ICAP_ServiceBus;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UseAzureServiceBusPublisher(builder.Configuration);
builder.Services.UseAzureServiceBusHandler(builder.Configuration);

builder.Services.AddSingleton(serviceProvider =>
{
    var mongoClient = new MongoClient(builder.Configuration.GetValue<string>("MongoURL"));
    return mongoClient.GetDatabase("Account");
});

builder.Services.AddSingleton<FriendRequestAccepted>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
