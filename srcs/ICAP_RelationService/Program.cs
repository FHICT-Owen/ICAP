using ICAP_Infrastructure.Repositories;
using ICAP_RelationService.Entities;
using ICAP_ServiceBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
