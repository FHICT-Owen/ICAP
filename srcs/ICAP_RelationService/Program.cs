using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using ICAP_ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UseAzureServiceBusPublisher(builder.Configuration);
builder.Services.UseAzureServiceBusHandler(builder.Configuration);

builder.Services.AddMongo()
    .AddMongoRepository<FriendRequest>("friendrequests")
    .AddMongoRepository<Friends>("friendlists");

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
